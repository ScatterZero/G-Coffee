//using G_Coffee_Services.IServices;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//[Route("api/[controller]")]
//[ApiController]
//public class PaymentController : ControllerBase
//{
//    private readonly IPayOSService _payOSService;
//    private readonly IRepository _repository;
//    private readonly string _checksumKey;

//    public PaymentController(IPayOSService payOSService, IRepository repository, IConfiguration config)
//    {
//        _payOSService = payOSService;
//        _repository = repository;
//        _checksumKey = config["PayOS:ChecksumKey"];
//    }

//    [HttpPost("create-payment-link/{comboId}")]
//    public async Task<IActionResult> CreatePaymentLink(int comboId, [FromBody] PaymentRequest request)
//    {
//        try
//        {
//            // Kiểm tra gói combo
//            var combo = await _repository.GetComboPackageByIdAsync(comboId);
//            if (combo == null)
//                return NotFound(new { Message = "Combo package not found" });

//            // Tạo đơn hàng
//            var order = new Order
//            {
//                OrderCode = request.OrderCode,
//                ComboPackageId = comboId,
//                Amount = combo.Price,
//                Status = "Pending",
//                CreatedAt = DateTime.UtcNow
//            };
//            await _repository.AddOrderAsync(order);
//            await _repository.SaveChangesAsync();

//            // Tạo link thanh toán
//            request.Amount = combo.Price;
//            request.Description = $"Thanh toán gói {combo.Name}";
//            var response = await _payOSService.CreatePaymentLink(request);
//            order.CheckoutUrl = response.CheckoutUrl;
//            await _repository.UpdateOrderAsync(order);
//            await _repository.SaveChangesAsync();

//            return Ok(new { CheckoutUrl = response.CheckoutUrl });
//        }
//        catch (Exception ex)
//        {
//            return BadRequest(new { Message = ex.Message });
//        }
//    }

//    [HttpPost("webhook")]
//    public async Task<IActionResult> Webhook([FromBody] WebhookData webhookData)
//    {
//        try
//        {
//            // Xác minh chữ ký (giả lập cho đồ án)
//            if (!VerifyWebhookSignature(webhookData, _checksumKey))
//                return BadRequest(new { Message = "Invalid signature" });

//            // Cập nhật trạng thái đơn hàng
//            var order = await _repository.GetOrderByOrderCodeAsync(webhookData.OrderCode);
//            if (order != null)
//            {
//                order.Status = webhookData.Status;
//                if (webhookData.Status == "PAID")
//                {
//                    // Kích hoạt gói combo (giả lập)
//                }
//                await _repository.UpdateOrderAsync(order);
//                await _repository.SaveChangesAsync();
//            }

//            return Ok(new { Message = "Webhook received" });
//        }
//        catch (Exception ex)
//        {
//            return BadRequest(new { Message = ex.Message });
//        }
//    }

//    private bool VerifyWebhookSignature(WebhookData data, string checksumKey)
//    {
//        // TODO: Thêm logic HMAC-SHA256 (xem https://docs.payos.vn)
//        return true; // Giả lập cho đồ án
//    }
//}