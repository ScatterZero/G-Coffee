using G_Cofee_Repositories.DTO;
using G_Cofee_Repositories.Models;
using G_Coffee_Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
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
        _config = config;
        _checksumKey = _config["PayOS:ChecksumKey"];
    }

    [HttpPost("create-payment-link/{orderId}")]
    public async Task<IActionResult> CreatePaymentLink(Guid orderId, [FromBody] PaymentRequest request)
    {
        var order = await _orderService.GetOrderByIdAsync(orderId);
        if (order == null)
            return NotFound();

        if (order.Status != "PENDING")
            return BadRequest(new { Message = "Chỉ xử lý đơn hàng PENDING" });

        request.OrderCode = order.OrderCode != 0 ? order.OrderCode : DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        request.Amount = order.Amount;
        request.Description ??= $"Thanh toán đơn hàng {orderId}";
        request.CancelUrl ??= _config["PayOS:CancelUrl"];
        request.ReturnUrl ??= _config["PayOS:ReturnUrl"];

        var response = await _payOSService.CreatePaymentLink(request);

        if (string.IsNullOrEmpty(response.CheckoutUrl))
            return BadRequest(new { Message = "Không thể tạo checkout URL từ PayOS" });

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
            if (!VerifyWebhookSignature(webhookData, _checksumKey))
                return BadRequest(new { Message = "Invalid signature" });

            var order = await _orderService.GetOrderByIdAsync(webhookData.OrderId);
            if (order != null)
            {
                order.Status = webhookData.Status;

                if (webhookData.Status == "PAID")
                {
                    var paymentDTO = new PaymentDTO
                    {
                        OrderId = webhookData.OrderId,
                        Amount = webhookData.Amount,
                        PaymentMethod = "PayOS",
                        PaymentDate = DateTime.Now,
                        Status = "PAID"
                    };

                    await _payOSService.CreatePaymentAsync(paymentDTO);
                }

                await _orderService.UpdateOrderAsync(order);
            }

            return Ok(new { Message = "Webhook received" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpGet("webhook/Get")]
    public IActionResult GetWebhook()
    {
        return Ok(new { Message = "Webhook endpoint is alive!" });
    }

    [HttpGet("payment/cancel")]
    public IActionResult CancelPayment()
    {
        return new ViewResult
        {
            ViewName = "cancel"
        };
    }

    [HttpGet("payment/success")]
    public IActionResult SuccessPayment()
    {
        return new ViewResult
        {
            ViewName = "success"
        };
    }

    private bool VerifyWebhookSignature(WebhookData data, string secretKey)
    {
        // 1. Chuyển đối tượng WebhookData thành JSON
        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        // 2. Tạo byte[] từ json và secretKey
        var keyBytes = Encoding.UTF8.GetBytes(secretKey);
        var messageBytes = Encoding.UTF8.GetBytes(json);

        // 3. Tính toán HMAC SHA256
        using var hmac = new HMACSHA256(keyBytes);
        var hashBytes = hmac.ComputeHash(messageBytes);

        // 4. Convert thành chuỗi hex (chữ thường)
        var computedSignature = Convert.ToHexString(hashBytes).ToLower();

        // 5. Lấy chữ ký từ header gửi kèm
        var receivedSignature = Request.Headers["x-signature"].FirstOrDefault();

        // 6. So sánh
        return !string.IsNullOrEmpty(receivedSignature) &&
               receivedSignature.Equals(computedSignature, StringComparison.OrdinalIgnoreCase);
    }

}
