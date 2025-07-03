using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using static G_Cofee_Repositories.Models.User;

namespace G_Cofee_Repositories.Models;

public partial class GcoffeeDbContext : DbContext
{
    public GcoffeeDbContext()
    {
    }

    public GcoffeeDbContext(DbContextOptions<GcoffeeDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<TransactionDetail> TransactionDetails { get; set; }

    public virtual DbSet<UnitsOfMeasure> UnitsOfMeasures { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Warehouse> Warehouses { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<ComboPackage> ComboPackages { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        //modelBuilder.Entity<User>(entity =>
        //{
        //    entity.Property(e => e.Role)
        //        .HasConversion(
        //            v => v.ToString(), // Chuyển RoleEnum sang string
        //            v => (RoleEnum)Enum.Parse(typeof(RoleEnum), v)); // Chuyển string về RoleEnum
        //});

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.InventoryId).HasName("PK__Inventor__F5FDE6D395371765");

            entity.ToTable("Inventory");

            entity.HasIndex(e => e.ProductId, "IDX_Inventory_ProductID");

            entity.Property(e => e.InventoryId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("InventoryID");
            entity.Property(e => e.ProductId)
                .HasMaxLength(13)
                .IsUnicode(false);
            entity.Property(e => e.LastUpdated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Quantity)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.WarehouseId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("WarehouseID");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inventory__Wareh__59FA5E80");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A5863035B0D");

            entity.Property(e => e.PaymentId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("PaymentID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Pending");
         
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PaymentCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Payments__Create__7A672E12");

    


            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.PaymentUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__Payments__Update__7B5B524B");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductID).HasName("PK__Products__177800D296728DEB");

            entity.HasIndex(e => e.ProductID, "IDX_Products_ProductId");

            entity.Property(e => e.ProductID)
                .HasMaxLength(13)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsDisabled).HasDefaultValue(false);
            entity.Property(e => e.ProductName).HasMaxLength(100);
            entity.Property(e => e.ShortName).HasMaxLength(50);
            entity.Property(e => e.SupplierId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SupplierID");
            entity.Property(e => e.UnitOfMeasureId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UnitOfMeasureID");
            entity.Property(e => e.UnitPrice)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProductCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Products__Create__5165187F");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Products)
                .HasForeignKey(d => d.SupplierId)
                .HasConstraintName("FK__Products__Suppli__4F7CD00D");

            entity.HasOne(d => d.UnitOfMeasure).WithMany(p => p.Products)
                .HasForeignKey(d => d.UnitOfMeasureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__UnitOf__5070F446");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ProductUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__Products__Update__52593CB8");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("PK__Supplier__4BE66694DD378E29");

            entity.Property(e => e.SupplierId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SupplierID");
            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.ContactPerson).HasMaxLength(100);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IsDisabled).HasDefaultValue(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.SupplierName).HasMaxLength(100);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__55433A4BF0A9ADB7");

            entity.HasIndex(e => e.TransactionNumber, "IDX_Transactions_TransactionNumber");

            entity.HasIndex(e => e.TransactionNumber, "UQ__Transact__E733A2BFD789BA6C").IsUnique();

            entity.Property(e => e.TransactionId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("TransactionID");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Draft");
            entity.Property(e => e.SupplierId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SupplierID");
            entity.Property(e => e.TotalAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalQuantity)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TransactionNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TransactionType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TransactionCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Transacti__Creat__656C112C");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.SupplierId)
                .HasConstraintName("FK__Transacti__Suppl__6477ECF3");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.TransactionUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__Transacti__Updat__66603565");
        });

        modelBuilder.Entity<TransactionDetail>(entity =>
        {
            entity.HasKey(e => e.TransactionDetailId).HasName("PK__Transact__F2B27FE63411FC08");

            entity.ToTable(tb => tb.HasTrigger("TRG_UpdateInventory"));

            entity.Property(e => e.TransactionDetailId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("TransactionDetailID");
            entity.Property(e => e.ProductId)
                .HasMaxLength(13)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalPrice)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.WarehouseId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("WarehouseID");

            entity.HasOne(d => d.BarcodeNavigation).WithMany(p => p.TransactionDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Barco__6D0D32F4");

            entity.HasOne(d => d.Transaction).WithMany(p => p.TransactionDetails)
                .HasForeignKey(d => d.TransactionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Trans__6C190EBB");



            entity.HasOne(d => d.Warehouse).WithMany(p => p.TransactionDetails)
                .HasForeignKey(d => d.WarehouseId)
                .HasConstraintName("FK__Transacti__Wareh__6E01572D");
        });

        modelBuilder.Entity<UnitsOfMeasure>(entity =>
        {
            entity.HasKey(e => e.UnitOfMeasureId).HasName("PK__UnitsOfM__F36083115ED9A740");

            entity.ToTable("UnitsOfMeasure");

            entity.Property(e => e.UnitOfMeasureId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UnitOfMeasureID");

        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC2C74C238");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E4BBCA6858").IsUnique();

            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UserID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.IsDisabled).HasDefaultValue(false);
            entity.Property(e => e.Password)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.Role)
        .HasMaxLength(50)
        .IsUnicode(false)
        .HasConversion(
            v => v.ToString(),
            v => (RoleEnum)Enum.Parse(typeof(RoleEnum), v));
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });
        //----------------------------------------------------
        // Cấu hình ComboPackage
        modelBuilder.Entity<ComboPackage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ComboPackages__3214EC07");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
        });

        // Cấu hình Order
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Orders__3214EC07");
            entity.Property(e => e.OrderCode).IsRequired();
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Status).HasMaxLength(50).IsUnicode(false).HasDefaultValue("Pending");
            entity.Property(e => e.CheckoutUrl).HasMaxLength(500).IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime").HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.ComboPackage)
                  .WithMany()
                  .HasForeignKey(d => d.ComboPackageId)
                  .HasConstraintName("FK__Orders__ComboPackageID__12345678");


        });
        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey(e => e.WarehouseId).HasName("PK__Warehous__2608AFD95DAF5031");

            entity.Property(e => e.WarehouseId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("WarehouseID");
            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ManagerId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ManagerID");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.WarehouseName).HasMaxLength(100);



            entity.HasOne(d => d.Manager).WithMany(p => p.WarehouseManagers)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK__Warehouse__Manag__3D5E1FD2");

        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
