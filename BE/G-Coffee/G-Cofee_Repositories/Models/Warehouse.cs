using System;
using System.Collections.Generic;

namespace G_Cofee_Repositories.Models;

public partial class Warehouse
{
    public string WarehouseId { get; set; } = null!;

    public string WarehouseName { get; set; } = null!;

    public string? Address { get; set; }

    public string? ManagerId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual User? Manager { get; set; }

    public virtual ICollection<TransactionDetail> TransactionDetails { get; set; } = new List<TransactionDetail>();

}
