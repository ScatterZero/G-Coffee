using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace G_Cofee_Repositories.Models;

public partial class Transaction
{
    public Guid TransactionId { get; set; }

    public string TransactionNumber { get; set; } = null!;

    [ForeignKey("Order")]
    public Guid? OrderId { get; set; }

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

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Supplier? Supplier { get; set; }

    public virtual ICollection<TransactionDetail> TransactionDetails { get; set; } = new List<TransactionDetail>();

    public virtual User? UpdatedByNavigation { get; set; }

    public virtual Order? Order { get; set; }
}
