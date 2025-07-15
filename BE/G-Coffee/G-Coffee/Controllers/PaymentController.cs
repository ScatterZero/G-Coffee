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

        //[HttpPost("webhook")]
        //[HttpPost]
        //public async Task<IActionResult> Webhook()
        //{
        //    string body;
        //    using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
        //    {
        //        body = await reader.ReadToEndAsync();
        //    }
        //    JObject obj = JObject.Parse(body);
        //    JObject data = (JObject)obj["data"]!;
        //    string signature = obj["signature"]!.ToString();
        //    var isValid = _payOSService.IsValidData(data.ToString(), signature);

        //    if (!isValid)
        //    {
        //        return BadRequest("Invalid data signature");
        //    }

        //    var payload = JsonSerializer.Deserialize<PayOSWebhookRequest>(body);

        //    await _payOSService.HandleWebhook(payload!);
        //    return Ok("Webhook processed successfully");
        //}

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

        // Hàm HMAC SHA256 - đã chính xác 100%
        private string ComputeHmacSha256(string payload, string secretKey)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
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

        // Model để nhận request test
        [HttpPost("test-signature")]
        public IActionResult TestSignature([FromBody] SignatureTestRequest request)
        {
            try
            {
                // Kiểm tra input
                if (string.IsNullOrEmpty(request.DataToSign) || string.IsNullOrEmpty(request.ChecksumKey))
                    return BadRequest(new { Message = "DataToSign and ChecksumKey are required" });

                // Log input để debug
                Console.WriteLine($"=== TEST SIGNATURE DEBUG ===");
                Console.WriteLine($"Input DataToSign: '{request.DataToSign}' (Length: {request.DataToSign.Length})");
                Console.WriteLine($"Input ChecksumKey: '{request.ChecksumKey}' (Length: {request.ChecksumKey.Length})");

                // Tính toán chữ ký
                var computedSignature = ComputeHmacSha256(request.DataToSign, request.ChecksumKey);

                // Log kết quả
                Console.WriteLine($"ComputedSignature: {computedSignature}");
                Console.WriteLine($"ExpectedSignature (from request): {request.ExpectedSignature ?? "Not provided"}");
                Console.WriteLine($"Match: {computedSignature == (request.ExpectedSignature ?? "")}");

                // Trả về kết quả
                return Ok(new
                {
                    DataToSign = request.DataToSign,
                    ChecksumKey = request.ChecksumKey,
                    ComputedSignature = computedSignature,
                    ExpectedSignature = request.ExpectedSignature,
                    IsMatch = computedSignature == (request.ExpectedSignature ?? "")
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TestSignature error: {ex}");
                return BadRequest(new { Message = $"Error: {ex.Message}" });
            }
        }

        public class SignatureTestRequest
        {
            public string DataToSign { get; set; }
            public string ChecksumKey { get; set; }
            public string ExpectedSignature { get; set; }
        }

        // Hàm chuyển JToken thành chuỗi query string - Đã cải tiến
        private string ConvertToQueryString(JToken token)
        {
            if (token is JObject obj)
            {
                var dataValue = obj["data"]?.ToString();
                return $"data={dataValue}"; // Chỉ lấy giá trị "data" và thêm prefix "data="
            }
            return string.Empty;
        }
    }
}