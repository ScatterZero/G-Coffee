using G_Cofee_Repositories.DTO;
using G_Coffee_Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace G_Coffee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionDetailController : ControllerBase
    {
        private readonly ITransactionDetailService _TransactionDetailService;

        public TransactionDetailController(ITransactionDetailService TransactionDetailService)
        {
            _TransactionDetailService = TransactionDetailService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransactionDetail([FromBody] TransactionDetailDTO TransactionDetailDTO)
        {
            if (TransactionDetailDTO == null) return BadRequest("TransactionDetail cannot be null");
            var createdTransactionDetail = await _TransactionDetailService.CreateTransactionDetailAsync(TransactionDetailDTO);
            return CreatedAtAction(nameof(GetTransactionDetail), new { id = createdTransactionDetail.TransactionDetailId }, createdTransactionDetail);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransactionDetail(string id)
        {
            var TransactionDetail = await _TransactionDetailService.GetTransactionDetailByIdAsync(id);
            if (TransactionDetail == null) return NotFound();
            return Ok(TransactionDetail);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransactionDetails()
        {
            var TransactionDetails = await _TransactionDetailService.GetAllTransactionDetailsAsync();
            return Ok(TransactionDetails);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransactionDetail(string id, [FromBody] TransactionDetailDTO TransactionDetailDto)
        {
            if (!Guid.TryParse(id, out var parsedId)) return BadRequest("Invalid TransactionDetail ID format");
            if (parsedId != TransactionDetailDto.TransactionDetailId) return BadRequest("TransactionDetail ID mismatch");
            await _TransactionDetailService.UpdateTransactionDetailAsync(TransactionDetailDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransactionDetail(string id)
        {
            await _TransactionDetailService.DeleteTransactionDetailAsync(id);
            return NoContent();
        }

        [HttpGet("TransactionDetail/{TransactionDetailId}")]
        public async Task<IActionResult> GetTransactionDetailsByTransactionDetailId(string TransactionDetailId)
        {
            var TransactionDetails = await _TransactionDetailService.GetTransactionDetailByIdAsync(TransactionDetailId);
            return Ok(TransactionDetails);
        }
    }
}
