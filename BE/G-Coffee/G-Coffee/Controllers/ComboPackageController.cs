using G_Cofee_Repositories.Models;
using G_Coffee_Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace G_Coffee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Yêu cầu xác thực cho toàn bộ controller
    public class ComboPackageController : ControllerBase
    {
        private readonly IComboPackageService _comboPackageService;

        public ComboPackageController(IComboPackageService comboPackageService)
        {
            _comboPackageService = comboPackageService;
        }

        [HttpPost]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> CreateComboPackage([FromBody] ComboPackage comboPackage)
        {
            try
            {
                if (comboPackage == null) return BadRequest("ComboPackage cannot be null");

                var created = await _comboPackageService.CreateComboPackageAsync(comboPackage);
                return CreatedAtAction(nameof(GetComboPackage), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User,Manager,Admin")]
        public async Task<IActionResult> GetComboPackage(string id)
        {
            try
            {
                var comboPackage = await _comboPackageService.GetComboPackageByIdAsync(id);
                return Ok(comboPackage);
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

        [HttpGet]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> GetAllComboPackages()
        {
            try
            {
                var list = await _comboPackageService.GetAllComboPackagesAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> UpdateComboPackage(string id, [FromBody] ComboPackage comboPackage)
        {
            try
            {
                if (!Guid.TryParse(id, out var parsedId))
                    return BadRequest("Invalid ComboPackage ID format");

                if (parsedId != comboPackage.Id)
                    return BadRequest("ComboPackage ID mismatch");

                await _comboPackageService.UpdateComboPackageAsync(comboPackage);
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteComboPackage(string id)
        {
            try
            {
                await _comboPackageService.DeleteComboPackageAsync(id);
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

        [HttpGet("ComboPackage/{comboPackageId}")]
        [Authorize(Roles = "User,Manager,Admin")]
        public async Task<IActionResult> GetComboPackagesByComboPackageId(string comboPackageId)
        {
            try
            {
                var combo = await _comboPackageService.GetComboPackageByIdAsync(comboPackageId);
                return Ok(combo);
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
