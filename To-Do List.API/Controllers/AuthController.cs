using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using To_Do_List.API.Models;
using To_Do_List.API.Repository;

namespace To_Do_List.API.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository) => _authRepository = authRepository;

        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserDTO request)
        {
            if(!await _authRepository.IsUsernameUnique(request.Username)) 
                return BadRequest("Username is already taken.");

            return Ok(await _authRepository.Register(request));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserDTO request)
        {
            var user = await _authRepository.AuthenticateUser(request.Username, request.Password);

            if (user is null)
                return BadRequest("Invalid username or password.");

            string token = _authRepository.CreateToken(user);

            return Ok(token);
        }

        [HttpGet("userinfo/{id}")]
        public async Task<IActionResult> UserInfo(string id)
        {
            if (id is null)
                return BadRequest("User identifier is not provided.");

            var user = await _authRepository.GetUser(id);

            return Ok(user);
        }
    }
}
