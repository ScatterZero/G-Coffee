using G_Cofee_Repositories.DTO;
using G_Coffee_Services.IServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace G_Coffee_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _transactionService.ImportReceipt(transaction);
                return Ok(new { Message = "Import transaction processed successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing the import.", Error = ex.Message });
            }
        }

        [HttpPost("export")]
        public async Task<IActionResult> ExportReceipt([FromBody] TransactionDTO transaction)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _transactionService.ExportReceipt(transaction);
                return Ok(new { Message = "Export transaction processed successfully.", Transaction = result });
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
                return StatusCode(500, new { Message = "An error occurred while processing the export.", Error = ex.Message });
            }
        }
    }
}