using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace G_Cofee_Repositories.DTO
{
    public class PayOSWebhookData
    {
        [JsonPropertyName("orderCode")]
        public long OrderCode { get; set; } = 0;

        [JsonPropertyName("amount")]
        public long Amount { get; set; } = 0;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("accountNumber")]
        public string AccountNumber { get; set; } = string.Empty;

        [JsonPropertyName("reference")]
        public string Reference { get; set; } = string.Empty;

        [JsonPropertyName("transactionDateTime")]
        public string TransactionDateTime { get; set; } = string.Empty;

        [JsonPropertyName("currency")]
        public string Currency { get; set; } = "VND";

        [JsonPropertyName("paymentLinkId")]
        public string PaymentLinkId { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public string Code { get; set; } = "00";

        [JsonPropertyName("desc")]
        public string Desc { get; set; } = "Thành công";

        [JsonPropertyName("counterAccountBankId")]
        public string CounterAccountBankId { get; set; } = string.Empty;

        [JsonPropertyName("counterAccountBankName")]
        public string CounterAccountBankName { get; set; } = string.Empty;

        [JsonPropertyName("counterAccountName")]
        public string CounterAccountName { get; set; } = string.Empty;

        [JsonPropertyName("counterAccountNumber")]
        public string CounterAccountNumber { get; set; } = string.Empty;

        [JsonPropertyName("virtualAccountName")]
        public string VirtualAccountName { get; set; } = string.Empty;

        [JsonPropertyName("virtualAccountNumber")]
        public string VirtualAccountNumber { get; set; } = string.Empty;
    }
}
