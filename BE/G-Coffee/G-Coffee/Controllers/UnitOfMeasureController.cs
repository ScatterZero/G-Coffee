using G_Cofee_Repositories.DTO;
using G_Coffee_Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace G_Coffee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitOfMeasureController : ControllerBase
    {
        private readonly IUnitOfMeasureService _UnitOfMeasureService;

        public UnitOfMeasureController(IUnitOfMeasureService UnitOfMeasureService)
        {
            _UnitOfMeasureService = UnitOfMeasureService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUnitOfMeasure([FromBody] UnitOfMeasureDTO UnitOfMeasureDTO)
        {
            if (UnitOfMeasureDTO == null) return BadRequest("UnitOfMeasure cannot be null");
            var createdUnitOfMeasure = await _UnitOfMeasureService.CreateUnitOfMeasureAsync(UnitOfMeasureDTO);
            return CreatedAtAction(nameof(GetUnitOfMeasure), new { id = createdUnitOfMeasure.UnitOfMeasureId }, createdUnitOfMeasure);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUnitOfMeasure(string id)
        {
            var UnitOfMeasure = await _UnitOfMeasureService.GetUnitOfMeasureByIdAsync(id);
            if (UnitOfMeasure == null) return NotFound();
            return Ok(UnitOfMeasure);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUnitOfMeasures()
        {
            var UnitOfMeasures = await _UnitOfMeasureService.GetAllUnitOfMeasuresAsync();
            return Ok(UnitOfMeasures);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUnitOfMeasure(string id, [FromBody] UnitOfMeasureDTO UnitOfMeasureDto)
        {
            if (id != UnitOfMeasureDto.UnitOfMeasureId) return BadRequest("UnitOfMeasure ID mismatch");
            await _UnitOfMeasureService.UpdateUnitOfMeasureAsync(UnitOfMeasureDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnitOfMeasure(string id)
        {
            await _UnitOfMeasureService.DeleteUnitOfMeasureAsync(id);
            return NoContent();
        }

        [HttpGet("UnitOfMeasure/{UnitOfMeasureId}")]
        public async Task<IActionResult> GetUnitOfMeasuresByUnitOfMeasureId(string UnitOfMeasureId)
        {
            var UnitOfMeasures = await _UnitOfMeasureService.GetUnitOfMeasureByIdAsync(UnitOfMeasureId);
            return Ok(UnitOfMeasures);
        }
    }
}
