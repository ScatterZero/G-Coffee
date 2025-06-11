using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Cofee_Repositories.DTO
{
    public class TransactionDTO
    {
        public Guid TransactionId { get; set; }

        public string TransactionNumber { get; set; } = null!;

        public DateOnly TransactionDate { get; set; }

        public string? SupplierId { get; set; }

        public decimal? TotalQuantity { get; set; }

        public decimal? TotalAmount { get; set; }

        public string TransactionType { get; set; } = null!;

        public string? Status { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? UpdatedBy { get; set; }
        public List<TransactionDetailDTO> TransactionDetails { get; set; } = new List<TransactionDetailDTO>();

    }
}
