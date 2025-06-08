using System;
using System.Collections.Generic;

namespace G_Cofee_Repositories.Models;

public partial class TransactionDetail
{
    public Guid TransactionDetailId { get; set; }

    public Guid TransactionId { get; set; }

    public string ProductId { get; set; } = null!;

    public decimal Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal? TotalPrice { get; set; }

    public string? WarehouseId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual Product BarcodeNavigation { get; set; } = null!;

    public virtual User? CreatedByNavigation { get; set; }

    public virtual Transaction Transaction { get; set; } = null!;

    public virtual User? UpdatedByNavigation { get; set; }

    public virtual Warehouse? Warehouse { get; set; }
}
