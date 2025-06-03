using G_Cofee_Repositories.DTO;
using G_Cofee_Repositories.Models;
using G_Coffee_Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace G_Coffee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

        public class AccountController : ControllerBase
        {
            private readonly IAccountService _userService;

            public AccountController(IAccountService userService)
            {
                _userService = userService;
            }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO loginDto)
        {
            var user = await _userService.LoginAsync(loginDto);
            if (user == null)
                return Unauthorized();
            return Ok(new { user.UserId, user.Username, user.Role });
        }

        [HttpPost("register")]
            public async Task<IActionResult> Register([FromBody] UserRegisterDTO registerDto)
            {
                try
                {
                    await _userService.RegisterAsync(registerDto);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
}
