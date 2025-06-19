using G_Cofee_Repositories.DTO;
using G_Cofee_Repositories.Models;
using G_Coffee_Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace G_Coffee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComboPackageController : ControllerBase
    {
        private readonly IComboPackageService _ComboPackageService;

        public ComboPackageController(IComboPackageService ComboPackageService)
        {
            _ComboPackageService = ComboPackageService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateComboPackage([FromBody] ComboPackage ComboPackage)
        {
            if (ComboPackage == null) return BadRequest("ComboPackage cannot be null");
            var createdComboPackage = await _ComboPackageService.CreateComboPackageAsync(ComboPackage);
            return CreatedAtAction(nameof(GetComboPackage), new { id = createdComboPackage.Id }, createdComboPackage);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetComboPackage(string id)
        {
            var ComboPackage = await _ComboPackageService.GetComboPackageByIdAsync(id);
            if (ComboPackage == null) return NotFound();
            return Ok(ComboPackage);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComboPackages()
        {
            var ComboPackages = await _ComboPackageService.GetAllComboPackagesAsync();
            return Ok(ComboPackages);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComboPackage(string id, [FromBody] ComboPackage ComboPackage)
        {
            if (!Guid.TryParse(id, out var parsedId)) return BadRequest("Invalid ComboPackage ID format");
            if (parsedId != ComboPackage.Id) return BadRequest("ComboPackage ID mismatch");
            await _ComboPackageService.UpdateComboPackageAsync(ComboPackage);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComboPackage(string id)
        {
            await _ComboPackageService.DeleteComboPackageAsync(id);
            return NoContent();
        }

        [HttpGet("ComboPackage/{ComboPackageId}")]
        public async Task<IActionResult> GetComboPackagesByComboPackageId(string ComboPackageId)
        {
            var ComboPackages = await _ComboPackageService.GetComboPackageByIdAsync(ComboPackageId);
            return Ok(ComboPackages);
        }
    }
}
