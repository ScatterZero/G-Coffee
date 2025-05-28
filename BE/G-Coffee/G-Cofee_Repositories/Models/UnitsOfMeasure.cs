using System;
using System.Collections.Generic;

namespace G_Cofee_Repositories.Models;

public partial class UnitsOfMeasure
{
    public string UnitOfMeasureId { get; set; } = null!;

    public string UnitName { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual User? UpdatedByNavigation { get; set; }
}
