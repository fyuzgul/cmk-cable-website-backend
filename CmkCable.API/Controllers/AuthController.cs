using CmkCable.Business.Concrete;
using CmkCable.DataAccess.Abstract;
using CmkCable.DataAccess.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace CmkCable.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userService;
        private readonly TokenManager _tokenService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IConfiguration configuration, ILogger<AuthController> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _userService = new UserRepository();
            _tokenService = new TokenManager(_configuration);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest model)
        {
            try
            {
                _logger.LogInformation($"Login attempt for user: {model.Username}");

                var user = _userService.AuthenticateAsync(model.Username, model.Password);
                if (user == null)
                {
                    _logger.LogWarning($"Authentication failed for user: {model.Username}");
                    return Unauthorized(new { message = "Invalid credentials" });
                }

                var token = _tokenService.GenerateToken(user);
                _logger.LogInformation($"Token generated successfully for user: {model.Username}");
                
                // Token'ı kontrol et
                try
                {
                    var principal = _tokenService.ValidateToken(token);
                    _logger.LogInformation("Generated token validated successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Generated token validation failed: {ex.Message}");
                    return StatusCode(500, new { message = "Error generating token" });
                }

                return Ok(new { token = token });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Login error: {ex.Message}");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        public class LoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
