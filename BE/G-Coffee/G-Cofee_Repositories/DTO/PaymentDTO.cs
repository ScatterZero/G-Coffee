using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Cofee_Repositories.DTO
{
    public class PaymentDTO
    {
        public Guid PaymentId { get; set; }

        public Guid OrderId { get; set; }

        public decimal Amount { get; set; }

        public string? PaymentMethod { get; set; }

        public DateTime? PaymentDate { get; set; } = DateTime.Now;

        public string? Status { get; set; }
    }
}
