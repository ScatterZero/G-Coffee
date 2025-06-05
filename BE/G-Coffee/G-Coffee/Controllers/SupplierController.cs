using G_Cofee_Repositories.DTO;
using G_Coffee_Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace G_Coffee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSupplier([FromBody] SupplierDTO supplierDTO)
        {
            if (supplierDTO == null) return BadRequest("Supplier cannot be null");
            var createdSupplier = await  _supplierService.CreateSupplierAsync(supplierDTO);
            return CreatedAtAction(nameof(GetSupplier), new { id = createdSupplier.SupplierId }, createdSupplier);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupplier(string id)
        {
            var Supplier = await _supplierService.GetSupplierByIdAsync(id);
            if (Supplier == null) return NotFound();
            return Ok(Supplier);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSuppliers()
        {
            var Suppliers = await _supplierService.GetAllSuppliersAsync();
            return Ok(Suppliers);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplier(string id, [FromBody] SupplierDTO SupplierDto)
        {
            if (id != SupplierDto.SupplierId) return BadRequest("Supplier ID mismatch");
            await _supplierService.UpdateSupplierAsync(SupplierDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(string id)
        {
            await _supplierService.DeleteSupplierAsync(id);
            return NoContent();
        }

        [HttpGet("supplier/{supplierId}")]
        public async Task<IActionResult> GetSuppliersBySupplierId(string supplierId)
        {
            var Suppliers = await _supplierService.GetSupplierByIdAsync(supplierId);
            return Ok(Suppliers);
        }
    }
}
