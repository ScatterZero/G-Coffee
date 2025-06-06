using G_Cofee_Repositories.DTO;
using G_Coffee_Services.IServices;
using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO loginDto)
        {
            try
            {
                var token = await _userService.LoginAsync(loginDto);
                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Tên người dùng hoặc mật khẩu không đúng");
            }
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO registerDto)
        {
            try
            {
                await _userService.RegisterAsync(registerDto);
                return Ok("Đăng ký thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}