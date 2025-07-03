using System;
using System.Collections.Generic;

namespace G_Cofee_Repositories.Models;

public partial class User
{
    public enum RoleEnum
    {
        User,
        Manager,
        Admin
    }
    public string UserId { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? FullName { get; set; }

    public RoleEnum Role { get; set; } /*= null!;*/
    

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? IsDisabled { get; set; }

    public virtual ICollection<Payment> PaymentCreatedByNavigations { get; set; } = new List<Payment>();

    public virtual ICollection<Payment> PaymentUpdatedByNavigations { get; set; } = new List<Payment>();

    public virtual ICollection<Product> ProductCreatedByNavigations { get; set; } = new List<Product>();

    public virtual ICollection<Product> ProductUpdatedByNavigations { get; set; } = new List<Product>();

    public virtual ICollection<Transaction> TransactionCreatedByNavigations { get; set; } = new List<Transaction>();

    public virtual ICollection<Transaction> TransactionUpdatedByNavigations { get; set; } = new List<Transaction>();

    public virtual ICollection<Warehouse> WarehouseManagers { get; set; } = new List<Warehouse>();
}
