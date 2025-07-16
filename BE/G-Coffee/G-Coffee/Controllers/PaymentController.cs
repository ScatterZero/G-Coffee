using G_Cofee_Repositories.DTO;
using G_Cofee_Repositories.Models;
using G_Coffee_Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Net.payOS.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace G_Coffee_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPayOSService _payOSService;
        private readonly IOrderService _orderService;
        private readonly IComboPackageService _comboPackageService;
        private readonly IConfiguration _config;
        private readonly string _checksumKey;

        public PaymentController(
            IPayOSService payOSService,
            IOrderService orderService,
            IComboPackageService comboPackageService,
            IConfiguration config)
        {
            _payOSService = payOSService;
            _orderService = orderService;
            _comboPackageService = comboPackageService;
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _checksumKey = _config.GetValue<string>("PayOS:ChecksumKey")
                           ?? throw new KeyNotFoundException("PayOS:ChecksumKey not found in config");

            // Log cấu hình hiện tại để debug
            foreach (var kvp in _config.AsEnumerable())
            {
                Console.WriteLine($"{kvp.Key} = {kvp.Value}");
            }
        }

        //[HttpPost("create-payment-link/{orderId}")]
        //public async Task<IActionResult> CreatePaymentLink(Guid orderId, [FromBody] PaymentRequest request)
        //{
        //    var order = await _orderService.GetOrderByIdAsync(orderId);
        //    if (order == null) return NotFound();
        //    if (order.Status != "PENDING") return BadRequest(new { Message = "Chỉ xử lý đơn hàng PENDING" });

        //    request.OrderCode = order.OrderCode != 0
        //        ? order.OrderCode
        //        : DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        //    request.Amount = order.Amount;
        //    request.Description ??= $"Thanh toán đơn hàng {orderId}";
        //    request.CancelUrl ??= _config["PayOS:CancelUrl"];
        //    request.ReturnUrl ??= _config["PayOS:ReturnUrl"];

        //    var response = await _payOSService.CreatePaymentLink(request);
        //    if (string.IsNullOrEmpty(response.CheckoutUrl))
        //        return BadRequest(new { Message = "Không thể tạo checkout URL từ PayOS" });

        //    order.CheckoutUrl = response.CheckoutUrl;
        //    order.OrderCode = response.OrderCode;
        //    await _orderService.UpdateOrderAsync(order);

        //    return Ok(new { CheckoutUrl = response.CheckoutUrl });
        //}
        [HttpPost("create-link/{orderId}")]
        public async Task<IActionResult> CreatePaymentLink([FromRoute] int orderId)
        {
            var paymentLink = await _payOSService.CreatePaymentLink(orderId);
            return Ok(paymentLink);
        }
        [HttpGet("payment/cancel")]
        public IActionResult CancelPayment()
        {
            return new ViewResult { ViewName = "cancel" };
        }

        [HttpGet("payment/success")]
        public IActionResult SuccessPayment()
        {
            return new ViewResult { ViewName = "success" };
        }

        [HttpPost("webhook")]
        public async Task HandleWebhook([FromBody] WebhookType webhookData)
        {
            try
            {
                await _payOSService.HandlePaymentWebhook(webhookData);
            }
            catch (Exception ex)
            {
                throw new Exception("Error handling webhook", ex);
            }
        }

    }
}