using G_Cofee_Repositories.Models;
using G_Coffee_Services.IServices;
using Microsoft.Extensions.Configuration;
using Net.payOS;
using Net.payOS.Types;

public class PayOSService : IPayOSService
{
    private readonly PayOS _payOS;
    private readonly IConfiguration _config;

    public PayOSService(IConfiguration config)
    {
        _config = config;
        _payOS = new PayOS(
            _config["PayOS:ClientId"],
            _config["PayOS:ApiKey"],
            _config["PayOS:ChecksumKey"]
        );
    }

    public async Task<PaymentResponse> CreatePaymentLink(PaymentRequest request)
    {
        request.CancelUrl ??= _config["PayOS:CancelUrl"];
        request.ReturnUrl ??= _config["PayOS:ReturnUrl"];

        // ✅ Rút gọn description tối đa 25 ký tự
        var desc = request.Description ?? $"DH {request.OrderCode}";
        if (desc.Length > 25)
        {
            desc = desc.Substring(0, 25);
        }

        var payOSRequest = new PaymentData(
            orderCode: request.OrderCode,
            amount: request.Amount,
            description: desc, // sử dụng desc đã rút gọn
            items: new List<ItemData>(), // Default empty list for 'items'
            cancelUrl: request.CancelUrl,
            returnUrl: request.ReturnUrl
        );

        var paymentLinkResponse = await _payOS.createPaymentLink(payOSRequest);

        return new PaymentResponse
        {
            CheckoutUrl = paymentLinkResponse.checkoutUrl,
            OrderCode = paymentLinkResponse.orderCode,
            Amount = request.Amount,
            Status = "PENDING"
        };
    }
}