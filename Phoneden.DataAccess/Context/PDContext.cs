namespace Phoneden.DataAccess.Context
{
  using Entities;
  using Extensions;
  using Microsoft.AspNetCore.Identity;
  using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore.Metadata;

  public class PdContext : IdentityDbContext<ApplicationUser>
  {
    public PdContext(DbContextOptions<PdContext> options)
      : base(options) { }

    public DbSet<Supplier> Suppliers { get; set; }

    public DbSet<Customer> Customers { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Brand> Brands { get; set; }

    public DbSet<Quality> Qualities { get; set; }

    public DbSet<SupplierContact> SupplierContacts { get; set; }

    public DbSet<CustomerContact> CustomerContacts { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<SupplierAddress> SupplierAddresses { get; set; }

    public DbSet<CustomerAddress> CustomerAddresses { get; set; }

    public DbSet<PurchaseOrder> PurchaseOrders { get; set; }

    public DbSet<PurchaseOrderInvoice> PurchaseOrderInvoices { get; set; }

    public DbSet<PurchaseOrderInvoiceLineItem> PurchaseOrderInvoiceLineItems { get; set; }

    public DbSet<PurchaseOrderInvoicePayment> PurchaseOrderInvoicePayments { get; set; }

    public DbSet<SaleOrder> SaleOrders { get; set; }

    public DbSet<SaleOrderLineItem> SaleOrderLineItems { get; set; }

    public DbSet<SaleOrderInvoice> SaleOrderInvoices { get; set; }

    public DbSet<SaleOrderInvoiceLineItem> SaleOrderInvoiceLineItems { get; set; }

    public DbSet<SaleOrderInvoicePayment> SaleOrderInvoicePayments { get; set; }

    public DbSet<SaleOrderReturn> SaleOrderReturns { get; set; }

    public DbSet<Expense> Expenses { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      foreach (IMutableEntityType entity in builder.Model.GetEntityTypes())
      {
        // replace table names
        string tableNameInSnakeCase = entity.GetTableName().ToSnakeCase();

        entity.SetTableName(tableNameInSnakeCase);

        // replace column names
        foreach (IMutableProperty property in entity.GetProperties())
        {
          string propertyColumnNameInSnakeCase = property.GetColumnName().ToSnakeCase();

          property.SetColumnName(propertyColumnNameInSnakeCase);
        }

        foreach (IMutableKey key in entity.GetKeys())
        {
          string keyNameInSnakeCase = key.GetName().ToSnakeCase();

          key.SetName(keyNameInSnakeCase);
        }

        foreach (IMutableForeignKey key in entity.GetForeignKeys())
        {
          string foreignKeyNameInSnakeCase = key.GetConstraintName().ToSnakeCase();

          key.SetConstraintName(foreignKeyNameInSnakeCase);
        }

        foreach (IMutableIndex index in entity.GetIndexes())
        {
          string indexInSnakeCase = index.GetName().ToSnakeCase();

          index.SetName(indexInSnakeCase);
        }
      }

      builder.Entity<Customer>()
        .HasMany(c => c.SaleOrders)
        .WithOne(e => e.Customer)
        .OnDelete(DeleteBehavior.Restrict);

      builder.Entity<Supplier>()
        .HasMany(po => po.PurchaseOrders)
        .WithOne(e => e.Supplier)
        .OnDelete(DeleteBehavior.Restrict);

      builder.Entity<PurchaseOrder>()
        .HasMany(li => li.LineItems)
        .WithOne(po => po.PurchaseOrder)
        .OnDelete(DeleteBehavior.Restrict);

      builder.Entity<SaleOrder>()
        .HasMany(li => li.LineItems)
        .WithOne(po => po.SaleOrder)
        .OnDelete(DeleteBehavior.Restrict);

      builder.Entity<ApplicationUser>(entity =>
      {
        entity.Property(m => m.Email).HasMaxLength(127);
        entity.Property(m => m.NormalizedEmail).HasMaxLength(127);
        entity.Property(m => m.NormalizedUserName).HasMaxLength(127);
        entity.Property(m => m.UserName).HasMaxLength(127);
      });

      builder.Entity<IdentityRole>(entity =>
      {
        entity.Property(m => m.Name).HasMaxLength(127);
        entity.Property(m => m.NormalizedName).HasMaxLength(127);
      });

      builder.Entity<IdentityUserLogin<string>>(entity =>
      {
        entity.Property(m => m.LoginProvider).HasMaxLength(127);
        entity.Property(m => m.ProviderKey).HasMaxLength(127);
      });

      builder.Entity<IdentityUserRole<string>>(entity =>
      {
        entity.Property(m => m.UserId).HasMaxLength(127);
        entity.Property(m => m.RoleId).HasMaxLength(127);
      });

      builder.Entity<IdentityUserToken<string>>(entity =>
      {
        entity.Property(m => m.UserId).HasMaxLength(127);
        entity.Property(m => m.LoginProvider).HasMaxLength(127);
        entity.Property(m => m.Name).HasMaxLength(127);
      });
    }
  }
}
