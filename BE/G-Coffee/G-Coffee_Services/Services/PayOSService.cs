using G_Cofee_Repositories.Models;
using G_Coffee_Services.IServices;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

public class PayOSService : IPayOSService
{
    private readonly HttpClient _httpClient;
    private readonly string _clientId;
    private readonly string _apiKey;

    public PayOSService(IConfiguration config, HttpClient httpClient)
    {
        _httpClient = httpClient;
        _clientId = config["PayOS:ClientId"];
        _apiKey = config["PayOS:ApiKey"];
        _httpClient.BaseAddress = new Uri(config["PayOS:BaseUrl"]);
    }

    public async Task<PaymentResponse> CreatePaymentLink(PaymentRequest request)
    {
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        _httpClient.DefaultRequestHeaders.Add("x-client-id", _clientId);
        _httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);

        var response = await _httpClient.PostAsync("/v2/payment-requests", content);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<PaymentResponse>(responseContent);
    }
}