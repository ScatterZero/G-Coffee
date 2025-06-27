using G_Cofee_Repositories.DTO;
using G_Coffee_Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace G_Coffee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Manager")] // Chỉ Admin hoặc Manager được truy cập
    public class WarehouseController : ControllerBase
    {
        private readonly IWarehouseService _WarehouseService;

        public WarehouseController(IWarehouseService WarehouseService)
        {
            _WarehouseService = WarehouseService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateWarehouse([FromBody] WarehouseDTO WarehouseDTO)
        {
            if (WarehouseDTO == null)
                return BadRequest("❌ Warehouse cannot be null.");

            try
            {
                var created = await _WarehouseService.CreateWarehouseAsync(WarehouseDTO);
                return CreatedAtAction(nameof(GetWarehouse), new { id = created.WarehouseId }, created);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"❌ Error creating warehouse: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous] // Cho phép truy cập public nếu cần
        public async Task<IActionResult> GetWarehouse(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Warehouse ID is required.");

            try
            {
                var warehouse = await _WarehouseService.GetWarehouseByIdAsync(id);
                if (warehouse == null)
                    return NotFound($"❌ Warehouse with ID {id} not found.");
                return Ok(warehouse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"❌ Error retrieving warehouse: {ex.Message}");
            }
        }

        [HttpGet]
        [AllowAnonymous] // Cho phép public xem danh sách nếu cần
        public async Task<IActionResult> GetAllWarehouses()
        {
            try
            {
                var list = await _WarehouseService.GetAllWarehousesAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"❌ Error retrieving warehouses: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWarehouse(string id, [FromBody] WarehouseDTO WarehouseDto)
        {
            if (!Guid.TryParse(id, out var parsedId))
                return BadRequest("Invalid Warehouse ID format.");

            if (parsedId.ToString() != WarehouseDto.WarehouseId)
                return BadRequest("Warehouse ID mismatch.");

            try
            {
                await _WarehouseService.UpdateWarehouseAsync(WarehouseDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Warehouse with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"❌ Error updating warehouse: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWarehouse(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Warehouse ID is required.");

            try
            {
                await _WarehouseService.DeleteWarehouseAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Warehouse with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"❌ Error deleting warehouse: {ex.Message}");
            }
        }

        [HttpGet("Warehouse/{WarehouseId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetWarehousesByWarehouseId(string WarehouseId)
        {
            if (string.IsNullOrWhiteSpace(WarehouseId))
                return BadRequest("Warehouse ID is required.");

            try
            {
                var result = await _WarehouseService.GetWarehouseByIdAsync(WarehouseId);
                if (result == null)
                    return NotFound($"Warehouse with ID {WarehouseId} not found.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"❌ Error retrieving warehouse: {ex.Message}");
            }
        }
    }
}
