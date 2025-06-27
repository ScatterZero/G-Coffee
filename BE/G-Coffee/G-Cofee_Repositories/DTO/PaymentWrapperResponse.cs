using G_Cofee_Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace G_Cofee_Repositories.DTO
{
    public class PaymentWrapperResponse
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("desc")]
        public string Description { get; set; }

        [JsonPropertyName("data")]
        public PaymentResponse Data { get; set; }
    }

}
