using System.Text.Json.Serialization;

namespace G_Cofee_Repositories.Models
{
    public class PaymentResponse
    {
        [JsonPropertyName("checkoutUrl")]
        public string? CheckoutUrl { get; set; }

        [JsonPropertyName("orderCode")]
        public long OrderCode { get; set; }

        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = "PENDING";
    }
}
