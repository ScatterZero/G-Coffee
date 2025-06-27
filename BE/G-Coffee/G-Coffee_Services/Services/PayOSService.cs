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

        // Fix for CS7036: Provide a default value for the 'items' parameter
        var payOSRequest = new PaymentData(
            orderCode: request.OrderCode,
            amount: request.Amount,
            description: request.Description,
            items: new List<ItemData>(), // Default empty list for 'items'
            cancelUrl: request.CancelUrl,
            returnUrl: request.ReturnUrl
        );

        // Fix for CS1061: Await the Task<CreatePaymentResult> to access its properties
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
