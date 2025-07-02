using G_Cofee_Repositories.DTO;
using G_Cofee_Repositories.Models;
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
        [HttpGet]
        public async Task<IActionResult> GetAllAccountAsync()
        {
            try
            {
                var list = await _userService.GetAllAccountsAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> UpdateAccount(string id, [FromBody] UserUpdateDTO Account)
        {
            try
            {
                if (!Guid.TryParse(id, out var parsedId))
                    return BadRequest("Invalid Account ID format");

                string idStr = parsedId.ToString(); // Chuyển Guid → string

                var user = await _userService.GetAccountByIdAsync(idStr); // Sử dụng string

                if (user == null || user.UserId != idStr)
                    return BadRequest("Account ID mismatch");

                await _userService.UpdateAccountAsync(idStr, Account); // Truyền id riêng vào
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out var parsedId))
                    return BadRequest("Invalid Account ID format");

                await _userService.DeleteAccountAsync(parsedId.ToString()); // Sử dụng string
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("Account/{AccountId}")]
        [Authorize(Roles = "User,Manager,Admin")]
        public async Task<IActionResult> GetAccountsByAccountId(string AccountId)
        {
            try
            {
                if (!Guid.TryParse(AccountId, out var parsedId))
                    return BadRequest("Invalid Account ID format");

                var account = await _userService.GetAccountByIdAsync(parsedId.ToString());
                return Ok(account);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}