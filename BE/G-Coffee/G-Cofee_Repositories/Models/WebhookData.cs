using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Cofee_Repositories.Models
{
    public class WebhookData
    {
        public Guid OrderId { get; set; }
        public string Status { get; set; } // PENDING, PAID, CANCELLED
        public int Amount { get; set; }
        public string Signature { get; set; } // Chữ ký xác minh
    }
}
