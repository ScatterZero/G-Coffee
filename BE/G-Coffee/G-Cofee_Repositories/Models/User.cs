using System;
using System.Collections.Generic;

namespace G_Cofee_Repositories.Models;

public partial class User
{
    public string UserId { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? FullName { get; set; }

    public string Role { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? IsDisabled { get; set; }

    public virtual ICollection<Payment> PaymentCreatedByNavigations { get; set; } = new List<Payment>();

    public virtual ICollection<Payment> PaymentUpdatedByNavigations { get; set; } = new List<Payment>();

    public virtual ICollection<Product> ProductCreatedByNavigations { get; set; } = new List<Product>();

    public virtual ICollection<Product> ProductUpdatedByNavigations { get; set; } = new List<Product>();

    public virtual ICollection<Supplier> SupplierCreatedByNavigations { get; set; } = new List<Supplier>();

    public virtual ICollection<Supplier> SupplierUpdatedByNavigations { get; set; } = new List<Supplier>();

    public virtual ICollection<Transaction> TransactionCreatedByNavigations { get; set; } = new List<Transaction>();

    public virtual ICollection<TransactionDetail> TransactionDetailCreatedByNavigations { get; set; } = new List<TransactionDetail>();

    public virtual ICollection<TransactionDetail> TransactionDetailUpdatedByNavigations { get; set; } = new List<TransactionDetail>();

    public virtual ICollection<Transaction> TransactionUpdatedByNavigations { get; set; } = new List<Transaction>();

    public virtual ICollection<UnitsOfMeasure> UnitsOfMeasureCreatedByNavigations { get; set; } = new List<UnitsOfMeasure>();

    public virtual ICollection<UnitsOfMeasure> UnitsOfMeasureUpdatedByNavigations { get; set; } = new List<UnitsOfMeasure>();

    public virtual ICollection<Warehouse> WarehouseCreatedByNavigations { get; set; } = new List<Warehouse>();

    public virtual ICollection<Warehouse> WarehouseManagers { get; set; } = new List<Warehouse>();

    public virtual ICollection<Warehouse> WarehouseUpdatedByNavigations { get; set; } = new List<Warehouse>();
}
