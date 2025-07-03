using System;
using System.Collections.Generic;

namespace G_Cofee_Repositories.Models;

public partial class UnitsOfMeasure
{
    public string UnitOfMeasureId { get; set; } = null!;

    public string UnitName { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

}
