using CmkCable.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CmkCable.Business.Concrete
{
    public class TokenManager
    {
        private readonly string _secretKey = "a-very-strong-secret-key-123456789"; // Güçlü anahtar
        private readonly string _issuer = "http://localhost:5972/";  // Uygulamanızın adresi
        private readonly string _audience = "http://localhost:5972/"; // Hedef adres

        public string GenerateToken(User user)
        {
            // Kullanıcı bilgilerinden token'a eklenecek claim'ler
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Benzersiz ID
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3), // Token süresi 3 saat
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Token'ı doğrulamak için bir metod ekleyelim
        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);

            try
            {
                // Token validation parametreleri
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true, // Token süresinin doğruluğunu kontrol et
                    ValidIssuer = _issuer,
                    ValidAudience = _audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero // Token süresi sıfırlanır
                }, out SecurityToken validatedToken);

                return principal; // Başarılı doğrulama
            }
            catch (SecurityTokenExpiredException)
            {
                throw new UnauthorizedAccessException("Token süresi dolmuş.");
            }
            catch (Exception)
            {
                throw new UnauthorizedAccessException("Geçersiz token.");
            }
        }
    }
}
