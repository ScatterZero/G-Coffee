using System;
using System.Collections.Generic;

namespace G_Cofee_Repositories.Models;

public partial class Payment
{
    public Guid PaymentId { get; set; }

    public Guid TransactionId { get; set; }

    public long OrderCode { get; set; }

    public decimal Amount { get; set; }

    public string? PaymentMethod { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual Transaction Transaction { get; set; } = null!;

    public virtual User? UpdatedByNavigation { get; set; }
    public virtual Order Order { get; set; } = null!;
}
