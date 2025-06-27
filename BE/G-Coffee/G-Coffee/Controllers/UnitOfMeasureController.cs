using G_Cofee_Repositories.DTO;
using G_Coffee_Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace G_Coffee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Toàn bộ controller yêu cầu xác thực JWT
    public class UnitOfMeasureController : ControllerBase
    {
        private readonly IUnitOfMeasureService _unitOfMeasureService;

        public UnitOfMeasureController(IUnitOfMeasureService unitOfMeasureService)
        {
            _unitOfMeasureService = unitOfMeasureService;
        }

        [HttpPost]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> CreateUnitOfMeasure([FromBody] UnitOfMeasureDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("UnitOfMeasure cannot be null");

                var created = await _unitOfMeasureService.CreateUnitOfMeasureAsync(dto);
                return CreatedAtAction(nameof(GetUnitOfMeasure), new { id = created.UnitOfMeasureId }, created);
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
        public async Task<IActionResult> GetUnitOfMeasure(string id)
        {
            try
            {
                var unit = await _unitOfMeasureService.GetUnitOfMeasureByIdAsync(id);
                return Ok(unit);
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
        public async Task<IActionResult> GetAllUnitOfMeasures()
        {
            try
            {
                var list = await _unitOfMeasureService.GetAllUnitOfMeasuresAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> UpdateUnitOfMeasure(string id, [FromBody] UnitOfMeasureDTO dto)
        {
            try
            {
                if (id != dto.UnitOfMeasureId)
                    return BadRequest("UnitOfMeasure ID mismatch");

                await _unitOfMeasureService.UpdateUnitOfMeasureAsync(dto);
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
        public async Task<IActionResult> DeleteUnitOfMeasure(string id)
        {
            try
            {
                await _unitOfMeasureService.DeleteUnitOfMeasureAsync(id);
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

        [HttpGet("UnitOfMeasure/{unitOfMeasureId}")]
        [Authorize(Roles = "User,Manager,Admin")]
        public async Task<IActionResult> GetUnitOfMeasuresByUnitOfMeasureId(string unitOfMeasureId)
        {
            try
            {
                var unit = await _unitOfMeasureService.GetUnitOfMeasureByIdAsync(unitOfMeasureId);
                return Ok(unit);
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
