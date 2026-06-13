using Microsoft.EntityFrameworkCore;
using TechTrack.API.Models;

namespace TechTrack.API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Transaction> Transactions => Set<Transaction>();


    // Fluent API configurations

    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);

        ConfigureUser(mb);
        ConfigureCategory(mb);
        ConfigureBrand(mb);
        ConfigureProduct(mb);
        ConfigureTransaction(mb);
    }

    //-------------------------------------------
    // USER
    //-------------------------------------------

    private static void ConfigureUser(ModelBuilder mb)
    {
        mb.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.HasKey(u => u.Id);

            entity.Property(u => u.Email)
                  .IsRequired()
                  .HasMaxLength(256);

            entity.HasIndex(u => u.Email).IsUnique();

            entity.Property(u => u.PasswordHash)
                    .IsRequired();

            entity.Property(u => u.Role)
                  .IsRequired()
                  .HasMaxLength(20)
                  .HasDefaultValue(AuthRole.Staff);

            entity.Property(u => u.CreatedAt)
                  .IsRequired()
                  .HasDefaultValueSql("Now()");

        });
    }

    //-----------------------------------------------
    // CATEGORY
    //-----------------------------------------------

    private static void ConfigureCategory(ModelBuilder mb)
    {
        mb.Entity<Category>(entity =>
        {
            entity.ToTable("categories");

            entity.HasKey(c => c.Id);

            entity.Property(c => c.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.HasIndex(c => c.Name)
                  .IsUnique();

            entity.Property(c => c.Description)
                  .HasMaxLength(500);
        });
    }

    //-----------------------------------------------
    // BRAND
    // ----------------------------------------------

    private static void ConfigureBrand(ModelBuilder mb)
    {
        mb.Entity<Brand>(entity =>
        {
            entity.ToTable("brands");

            entity.HasKey(b => b.Id);

            entity.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasIndex(b => b.Name)
                .IsUnique();

            entity.Property(b => b.ContactEmail)
                .HasMaxLength(256);

            entity.Property(b => b.Website)
                .HasMaxLength(256);
        });
    }

    //-----------------------------------------------
    // PRODUCT
    // ----------------------------------------------

    private static void ConfigureProduct(ModelBuilder mb)
    {
        mb.Entity<Product>(entity =>
        {
            entity.ToTable("products");

            entity.HasKey(b => b.Id);

            entity.Property(p => p.SKU)
            .IsRequired()
            .HasMaxLength(50);

            // Unique constraint on SKU
            entity.HasIndex(p => p.SKU)
                .IsUnique();

            entity.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

            entity.Property(p => p.Description)
            .HasMaxLength(1000);

            entity.Property(p => p.UnitCost)
            .IsRequired()
            .HasColumnType("numeric(10,2)");

            entity.Property(p => p.SellPrice)
            .IsRequired()
            .HasColumnType("numeric(10,2)");

            entity.Property(p => p.QuantityOnHand)
            .IsRequired()
            .HasDefaultValue(0);

            entity.Property(p => p.ReorderThreshold)
               .IsRequired()
               .HasDefaultValue(0);

            entity.Property(p => p.WarehouseLocation)
                .HasMaxLength(100);

            entity.Property(p => p.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW()");

            entity.Property(p => p.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW()");

            // --- Relationships ---

            // Many Products → One Category
            // If a Category is deleted, RESTRICT the delete (don't cascade-wipe products)
            entity.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Many Products → One Brand
            // Same pattern — protect products from accidental brand deletion
            entity.HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    // -------------------------------------------------------
    // TRANSACTION
    // -------------------------------------------------------
    private static void ConfigureTransaction(ModelBuilder mb)
    {
        mb.Entity<Transaction>(entity =>
        {
            entity.ToTable("transactions");

            entity.HasKey(t => t.Id);

            // Store the enum as a string in the DB ("Receipt", "Adjustment", "Outgoing")
            // Much easier to read in queries than integers
            entity.Property(t => t.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            entity.Property(t => t.Quantity)
                .IsRequired();

            entity.Property(t => t.Notes)
                .HasMaxLength(500);

            entity.Property(t => t.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW()");

            // --- Relationships ---

            // Many Transactions → One Product
            // Cascade delete: if a product is deleted, its transaction history goes too
            entity.HasOne(t => t.Product)
                .WithMany(p => p.Transactions)
                .HasForeignKey(t => t.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Many Transactions → One User
            // Restrict: never delete a user who has logged transactions (audit trail)
            entity.HasOne(t => t.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
