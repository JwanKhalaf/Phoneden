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

    public DbSet<Business> Businesses { get; set; }

    public DbSet<Supplier> Suppliers { get; set; }

    public DbSet<Customer> Customers { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Brand> Brands { get; set; }

    public DbSet<Quality> Qualities { get; set; }

    public DbSet<Contact> Contacts { get; set; }

    public DbSet<Partner> Partners { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<Address> Addresses { get; set; }

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
        entity.Relational().TableName = entity.Relational().TableName.ToSnakeCase();

        // replace column names
        foreach (IMutableProperty property in entity.GetProperties())
        {
          property.Relational().ColumnName = property.Name.ToSnakeCase();
        }

        foreach (IMutableKey key in entity.GetKeys())
        {
          key.Relational().Name = key.Relational().Name.ToSnakeCase();
        }

        foreach (IMutableForeignKey key in entity.GetForeignKeys())
        {
          key.Relational().Name = key.Relational().Name.ToSnakeCase();
        }

        foreach (IMutableIndex index in entity.GetIndexes())
        {
          index.Relational().Name = index.Relational().Name.ToSnakeCase();
        }
      }

      builder.Entity<Business>()
        .ToTable("businesses");

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
