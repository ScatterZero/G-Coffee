using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Cofee_Repositories.Models
{
    public class PaymentResponse
    {
        public long OrderCode { get; set; }
        public required string Status { get; set; } // PENDING, PAID, CANCELLED
        public int Amount { get; set; }
        public required string CheckoutUrl { get; set; } // URL thanh toán
    }
}
