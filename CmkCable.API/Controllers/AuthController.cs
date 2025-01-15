using CmkCable.Business.Concrete;
using CmkCable.DataAccess;
using CmkCable.DataAccess.Abstract;
using CmkCable.DataAccess.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Threading.Tasks;

namespace CmkCable.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userService;
        private readonly TokenManager _tokenService;

        public AuthController()
        {
            _userService = new UserRepository();
            _tokenService = new TokenManager();
            
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest model)
        {
            var user =  _userService.AuthenticateAsync(model.Username, model.Password);
            if (user == null)
                return Unauthorized("Invalid credentials");

            var token = _tokenService.GenerateToken(user);
            return Ok(new { token });
        }

        public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    }
}
