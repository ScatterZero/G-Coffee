using G_Cofee_Repositories.DTO;
using G_Cofee_Repositories.Models;
using G_Coffee_Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Net.payOS;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IPayOSService _payOSService;
    private readonly IOrderService _orderService;
    private readonly IComboPackageService _comboPackageService;
    private readonly IConfiguration _config;
    private readonly string _checksumKey;

    public PaymentController(IPayOSService payOSService, IOrderService orderService, IComboPackageService comboPackageService, IConfiguration config)
    {
        _payOSService = payOSService;
        _orderService = orderService;
        _comboPackageService = comboPackageService;
        _checksumKey = config["PayOS:ChecksumKey"];
    }

    [HttpPost("create-payment-link/{orderId}")]
    public async Task<IActionResult> CreatePaymentLink(Guid orderId, [FromBody] PaymentRequest request)
    {
        var order = await _orderService.GetOrderByIdAsync(orderId);
        if (order == null)
            return NotFound();

        if (order.Status != "PENDING")
            return BadRequest(new { Message = "Chỉ xử lý đơn hàng PENDING" });

        // Tạo orderCode nếu chưa có
        request.OrderCode = order.OrderCode != 0 ? order.OrderCode : DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        request.Amount = order.Amount;
        request.Description ??= $"Thanh toán đơn hàng {orderId}";
        request.CancelUrl ??= _config["PayOS:CancelUrl"];
        request.ReturnUrl ??= _config["PayOS:ReturnUrl"];

        var response = await _payOSService.CreatePaymentLink(request);

        if (string.IsNullOrEmpty(response.CheckoutUrl))
            return BadRequest(new { Message = "Không thể tạo checkout URL từ PayOS" });

        // Cập nhật CheckoutUrl trong đơn hàng

        order.CheckoutUrl = response.CheckoutUrl;
        order.OrderCode = response.OrderCode;
        await _orderService.UpdateOrderAsync(order);

        return Ok(new { CheckoutUrl = response.CheckoutUrl });
    }


    [HttpPost("webhook")]
    public async Task<IActionResult> Webhook([FromBody] WebhookData webhookData)
    {
        try
        {
            // Xác minh chữ ký (giả lập cho đồ án)
            if (!VerifyWebhookSignature(webhookData, _checksumKey))
                return BadRequest(new { Message = "Invalid signature" });

            // Cập nhật trạng thái đơn hàng
            var order = await _orderService.GetOrderByOrderCodeAsync(webhookData.OrderCode);
            if (order != null)
            {
                order.Status = webhookData.Status;
                if (webhookData.Status == "PAID")
                {
                    // Kích hoạt gói combo (giả lập)
                }
                await _orderService.UpdateOrderAsync(order);
                // Removed the call to SaveChangesAsync as it is not part of IOrderService
            }

            return Ok(new { Message = "Webhook received" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    private bool VerifyWebhookSignature(WebhookData data, string checksumKey)
    {
        // TODO: Thêm logic HMAC-SHA256 (xem https://docs.payos.vn)
        return true; // Giả lập cho đồ án
    }
    [HttpGet("payment/cancel")]
    public IActionResult CancelPayment()
    {
        return new ViewResult
        {
            ViewName = "cancel"
        }; // ASP.NET Core will automatically look for Views/Payment/cancel.cshtml
    }
    [HttpGet("payment/success")]
    public IActionResult SuccessPayment()
    {
        return new ViewResult
        {
            ViewName = "success"
        }; // ASP.NET Core will automatically look for Views/Payment/cancel.cshtml
    }

}