using G_Cofee_Repositories.DTO;
using G_Cofee_Repositories.Models;
using G_Coffee_Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

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

        [HttpPost("create-payment-link/{orderId}")]
        public async Task<IActionResult> CreatePaymentLink(Guid orderId, [FromBody] PaymentRequest request)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null) return NotFound();
            if (order.Status != "PENDING") return BadRequest(new { Message = "Chỉ xử lý đơn hàng PENDING" });

            request.OrderCode = order.OrderCode != 0
                ? order.OrderCode
                : DateTimeOffset.UtcNow.ToUnixTimeSeconds();

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
        public async Task<IActionResult> Webhook()
        {
            try
            {
                // Đọc toàn bộ body của request
                Request.EnableBuffering();
                using var reader = new StreamReader(Request.Body, Encoding.UTF8, leaveOpen: true);
                var rawBody = await reader.ReadToEndAsync();
                Request.Body.Position = 0;

                // Parse JSON payload
                JObject payload;
                try
                {
                    payload = JObject.Parse(rawBody);
                }
                catch (System.Text.Json.JsonException)
                {
                    return BadRequest(new { Message = "Invalid JSON payload" });
                }

                // Lấy chữ ký nhận được
                var receivedSignature = payload["signature"]?.ToString();
                if (string.IsNullOrEmpty(receivedSignature))
                    return BadRequest(new { Message = "Signature is missing" });

                // Loại bỏ trường signature để tính toán chữ ký
                payload.Remove("signature");

                // Sắp xếp các key theo thứ tự bảng chữ cái và chuyển thành chuỗi query
                var sortedPayload = SortJToken(payload);
                var dataToSign = ConvertToQueryString(sortedPayload);

                // Tính toán chữ ký HMAC-SHA256
                var computedSignature = ComputeHmacSha256(dataToSign, _checksumKey);

                // Log để debug
                Console.WriteLine($"RawBody: {rawBody}");
                Console.WriteLine($"DataToSign: {dataToSign}");
                Console.WriteLine($"ComputedSignature: {computedSignature}");
                Console.WriteLine($"ReceivedSignature: {receivedSignature}");

                // So sánh chữ ký
                if (!receivedSignature.Equals(computedSignature, StringComparison.OrdinalIgnoreCase))
                    return BadRequest(new { Message = "Invalid signature" });

                // Parse và deserialize dữ liệu từ trường "data"
                var dataJson = payload["data"]?.ToString();
                if (string.IsNullOrEmpty(dataJson))
                    return BadRequest(new { Message = "Invalid data: 'data' property is null or empty" });

                // Parse chuỗi JSON trong dataJson
                JObject dataObject;
                try
                {
                    dataObject = JObject.Parse(dataJson);
                }
                catch (System.Text.Json.JsonException)
                {
                    return BadRequest(new { Message = "Invalid data format" });
                }

                var data = dataObject.ToObject<WebhookData>(new Newtonsoft.Json.JsonSerializer
                {
                    ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
                });
                if (data == null) return BadRequest(new { Message = "Invalid data" });

                // Kiểm tra đơn hàng
                var order = await _orderService.GetOrderByOrderCodeAsync(data.OrderCode);
                if (order == null) return NotFound();

                order.Status = data.Status;

                // Xử lý khi thanh toán thành công
                if (data.Status == "PAID")
                {
                    var paymentDTO = new PaymentDTO
                    {
                        OrderId = order.Id,
                        Amount = data.Amount,
                        PaymentMethod = "PayOS",
                        PaymentDate = DateTime.Now,
                        Status = "PAID"
                    };
                    await _payOSService.CreatePaymentAsync(paymentDTO);
                }

                await _orderService.UpdateOrderAsync(order);
                return Ok(new { Message = "Webhook processed" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Webhook error: {ex}");
                return BadRequest(new { Message = "An error occurred while processing the webhook" });
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
            return new ViewResult { ViewName = "cancel" };
        }

        [HttpGet("payment/success")]
        public IActionResult SuccessPayment()
        {
            return new ViewResult { ViewName = "success" };
        }

        private string ComputeHmacSha256(string payload, string secretKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var payloadBytes = Encoding.UTF8.GetBytes(payload);

            using var hmac = new HMACSHA256(keyBytes);
            var hash = hmac.ComputeHash(payloadBytes);

            return Convert.ToHexString(hash).ToLower();
        }

        // Hàm sắp xếp JToken theo thứ tự bảng chữ cái
        private JToken SortJToken(JToken token)
        {
            if (token is JObject obj)
            {
                var sortedObj = new JObject();
                foreach (var prop in obj.Properties().OrderBy(p => p.Name))
                {
                    sortedObj.Add(prop.Name, SortJToken(prop.Value));
                }
                return sortedObj;
            }
            else if (token is JArray array)
            {
                return new JArray(array.Select(SortJToken));
            }
            return token;
        }
        [HttpPost("calculate-signature")]
        public IActionResult CalculateSignature([FromBody] SignatureRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.DataToSign) || string.IsNullOrEmpty(request.ChecksumKey))
                    return BadRequest(new { Message = "DataToSign and ChecksumKey are required" });

                var computedSignature = ComputeHmacSha256(request.DataToSign, request.ChecksumKey);
                return Ok(new { ComputedSignature = computedSignature });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Error calculating signature: {ex.Message}" });
            }
        }

        // Model để nhận request
        public class SignatureRequest
        {
            public string DataToSign { get; set; }
            public string ChecksumKey { get; set; }
        }

        // Hàm chuyển JToken thành chuỗi query string
        private string ConvertToQueryString(JToken token)
        {
            if (token is JObject obj)
            {
                var pairs = new List<string>();
                foreach (var prop in obj.Properties())
                {
                    var value = prop.Value is JObject || prop.Value is JArray
                        ? JsonConvert.SerializeObject(SortJToken(prop.Value), Formatting.None)
                        : prop.Value.ToString();
                    pairs.Add($"{prop.Name}={value}");
                }
                return string.Join("&", pairs.OrderBy(p => p));
            }
            return string.Empty;
        }
    }
}