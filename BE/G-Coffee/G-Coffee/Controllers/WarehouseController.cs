using G_Cofee_Repositories.DTO;
using G_Coffee_Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace G_Coffee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            if (WarehouseDTO == null) return BadRequest("Warehouse cannot be null");
            var createdWarehouse = await _WarehouseService.CreateWarehouseAsync(WarehouseDTO);
            return CreatedAtAction(nameof(GetWarehouse), new { id = createdWarehouse.WarehouseId }, createdWarehouse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWarehouse(string id)
        {
            var Warehouse = await _WarehouseService.GetWarehouseByIdAsync(id);
            if (Warehouse == null) return NotFound();
            return Ok(Warehouse);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWarehouses()
        {
            var Warehouses = await _WarehouseService.GetAllWarehousesAsync();
            return Ok(Warehouses);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWarehouse(string id, [FromBody] WarehouseDTO WarehouseDto)
        {
            if (!Guid.TryParse(id, out var parsedId)) return BadRequest("Invalid Warehouse ID format");
            if (parsedId.ToString() != WarehouseDto.WarehouseId) return BadRequest("Warehouse ID mismatch");
            await _WarehouseService.UpdateWarehouseAsync(WarehouseDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWarehouse(string id)
        {
            await _WarehouseService.DeleteWarehouseAsync(id);
            return NoContent();
        }

        [HttpGet("Warehouse/{WarehouseId}")]
        public async Task<IActionResult> GetWarehousesByWarehouseId(string WarehouseId)
        {
            var Warehouses = await _WarehouseService.GetWarehouseByIdAsync(WarehouseId);
            return Ok(Warehouses);
        }
    }
}
