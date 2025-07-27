using G_Cofee_Repositories.DTO;
using G_Coffee_Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace G_Coffee_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Manager")] // Chỉ Admin và Manager có thể nhập / xuất hàng
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportReceipt([FromBody] TransactionDTO transaction)
        {
            if (transaction == null)
                return BadRequest(new { Message = "Transaction cannot be null." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                 var result = await _transactionService.ImportReceipt(transaction);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "❌ Internal Server Error while importing transaction.", Error = ex.Message });
            }
        }

        [HttpPost("export")]
        public async Task<IActionResult> ExportReceipt([FromBody] TransactionDTO transaction)
        {
            if (transaction == null)
                return BadRequest(new { Message = "Transaction cannot be null." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _transactionService.ExportReceipt(transaction);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "❌ Internal Server Error while exporting transaction.", Error = ex.Message });
            }
        }
        [HttpGet("get-all-transactions")]
        public async Task<IActionResult> GetAllTransactions()
        {
            try
            {
                var transactions = await _transactionService.GetAllTransactionsAsync();
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "❌ Internal Server Error while fetching transactions.", Error = ex.Message });
            }
        }
        [HttpGet("get-transaction-by-id/{transactionId}")]
        public async Task<IActionResult> GetTransactionById(string transactionId)
        {
            if (string.IsNullOrEmpty(transactionId))
                return BadRequest(new { Message = "Transaction ID cannot be null or empty." });
            try
            {
                var transaction = await _transactionService.GetTransactionByIdAsync(transactionId);
                if (transaction == null)
                    return NotFound(new { Message = "Transaction not found." });
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "❌ Internal Server Error while fetching transaction.", Error = ex.Message });
            }
        }
        [HttpDelete("delete-transaction/{transactionId}")]
        public async Task<IActionResult> DeleteTransaction(string transactionId)
        {
            if (string.IsNullOrEmpty(transactionId))
                return BadRequest(new { Message = "Transaction ID cannot be null or empty." });
            try
            {
                await _transactionService.DeleteTransactionAsync(transactionId);
                return Ok(new { Message = "Transaction deleted successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "❌ Internal Server Error while deleting transaction.", Error = ex.Message });
            }
        }
        [HttpPut("update-transaction/{transactionId}")]
        public async Task<IActionResult> UpdateTransaction(string transactionId, [FromBody] UpdateTransactionDTO transactionDto)
        {
            if (string.IsNullOrEmpty(transactionId))
                return BadRequest(new { Message = "Transaction ID cannot be null or empty." });
            if (transactionDto == null)
                return BadRequest(new { Message = "Transaction data cannot be null." });
            try
            {
                var updatedTransaction = await _transactionService.UpdateTransactionAsync(transactionId, transactionDto);
                return Ok(updatedTransaction);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "❌ Internal Server Error while updating transaction.", Error = ex.Message });
            }
        }

    }
}
