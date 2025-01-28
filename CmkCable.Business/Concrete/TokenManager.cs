using CmkCable.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace CmkCable.Business.Concrete
{
    public class TokenManager
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly IConfiguration _configuration;

        public TokenManager(IConfiguration configuration)
        {
            _configuration = configuration;
            _secretKey = _configuration["Jwt:SecretKey"];
            _issuer = _configuration["Jwt:Issuer"];
            _audience = _configuration["Jwt:Audience"];
        }

        public string GenerateToken(User user)
        {
            if (string.IsNullOrEmpty(_secretKey) || string.IsNullOrEmpty(_issuer) || string.IsNullOrEmpty(_audience))
            {
                throw new InvalidOperationException("JWT configuration is missing or invalid");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            if (key.KeySize < 256)
            {
                throw new InvalidOperationException("The secret key must be at least 32 bytes long");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Username)
            };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var now = DateTime.UtcNow;

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                notBefore: now,
                expires: now.AddHours(3),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException(nameof(token));

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudience = _audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
                RequireSignedTokens = true
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                if (!(validatedToken is JwtSecurityToken jwtToken) || 
                    !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token algorithm");
                }

                return principal;
            }
            catch (SecurityTokenExpiredException)
            {
                throw new SecurityTokenExpiredException("Token has expired");
            }
            catch (SecurityTokenInvalidSignatureException)
            {
                throw new SecurityTokenInvalidSignatureException("Invalid token signature");
            }
            catch (SecurityTokenInvalidIssuerException)
            {
                throw new SecurityTokenInvalidIssuerException("Invalid token issuer");
            }
            catch (SecurityTokenInvalidAudienceException)
            {
                throw new SecurityTokenInvalidAudienceException("Invalid token audience");
            }
            catch (Exception ex)
            {
                throw new SecurityTokenException($"Token validation failed: {ex.Message}");
            }
        }
    }
}
