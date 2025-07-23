using System;
using System.Collections.Generic;

namespace G_Cofee_Repositories.Models;

public partial class Inventory
{
    public Guid InventoryId { get; set; } = new Guid();

    public string WarehouseId { get; set; } = null!;

    public string ProductID { get; set; } = null!;

    public decimal? Quantity { get; set; }

    public DateTime? LastUpdated { get; set; }
    public int Min { get; set; }

    public int Max { get; set; }   


    public virtual Warehouse Warehouse { get; set; } = null!;

}
