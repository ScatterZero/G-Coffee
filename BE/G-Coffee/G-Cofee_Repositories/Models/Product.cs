using System;
using System.Collections.Generic;

namespace G_Cofee_Repositories.Models;

public partial class Product
{
    public string Barcode { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public string? ShortName { get; set; }

    public string UnitOfMeasureId { get; set; } = null!;

    public decimal? UnitPrice { get; set; }

    public string? SupplierId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public bool? IsDisabled { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual Supplier? Supplier { get; set; }

    public virtual ICollection<TransactionDetail> TransactionDetails { get; set; } = new List<TransactionDetail>();

    public virtual UnitsOfMeasure UnitOfMeasure { get; set; } = null!;

    public virtual User? UpdatedByNavigation { get; set; }
}
