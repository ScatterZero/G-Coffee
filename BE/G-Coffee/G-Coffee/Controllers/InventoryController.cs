using G_Cofee_Repositories.DTO;
using G_Coffee_Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace G_Coffee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _InventoryService;

        public InventoryController(IInventoryService InventoryService)
        {
            _InventoryService = InventoryService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateInventory([FromBody] InventoryDTO InventoryDTO)
        {
            if (InventoryDTO == null) return BadRequest("Inventory cannot be null");
            var createdInventory = await _InventoryService.CreateInventoryAsync(InventoryDTO);
            return CreatedAtAction(nameof(GetInventory), new { id = createdInventory.InventoryId }, createdInventory);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInventory(string id)
        {
            var Inventory = await _InventoryService.GetInventoryByIdAsync(id);
            if (Inventory == null) return NotFound();
            return Ok(Inventory);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInventorys()
        {
            var Inventorys = await _InventoryService.GetAllInventorysAsync();
            return Ok(Inventorys);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInventory(string id, [FromBody] InventoryDTO InventoryDto)
        {
            if (!Guid.TryParse(id, out var parsedId)) return BadRequest("Invalid Inventory ID format");
            if (parsedId != InventoryDto.InventoryId) return BadRequest("Inventory ID mismatch");
            await _InventoryService.UpdateInventoryAsync(InventoryDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventory(string id)
        {
            await _InventoryService.DeleteInventoryAsync(id);
            return NoContent();
        }

        [HttpGet("Inventory/{InventoryId}")]
        public async Task<IActionResult> GetInventorysByInventoryId(string InventoryId)
        {
            var Inventorys = await _InventoryService.GetInventoryByIdAsync(InventoryId);
            return Ok(Inventorys);
        }
    }
}
