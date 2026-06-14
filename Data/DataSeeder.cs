using Microsoft.EntityFrameworkCore;
using TechTrack.API.Models;

namespace TechTrack.API.Data;

public static class DataSeeder
{
    // -------------------------------------------------------
    // SYNC — called by EF Core tooling (dotnet ef commands)
    // -------------------------------------------------------
    public static void Seed(DbContext context)
    {
        SeedUsers(context);
        SeedCategories(context);
        SeedBrands(context);
        SeedProducts(context);
        SeedTransactions(context);
        context.SaveChanges();
    }

    // -------------------------------------------------------
    // ASYNC — called at application startup
    // -------------------------------------------------------
    public static async Task SeedAsync(DbContext context, CancellationToken ct = default)
    {
        SeedUsers(context);
        SeedCategories(context);
        SeedBrands(context);
        SeedProducts(context);
        SeedTransactions(context);
        await context.SaveChangesAsync(ct);
    }

    // -------------------------------------------------------
    // USERS
    // -------------------------------------------------------
    private static void SeedUsers(DbContext context)
    {
        // Only seed if the table is empty — prevents duplicate inserts on restart
        if (context.Set<User>().Any()) return;

        context.Set<User>().AddRange(
            new User
            {
                Email = "admin@novadist.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@1234"),
                Role = "Admin",
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                Email = "staff@novadist.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Staff@1234"),
                Role = "Staff",
                CreatedAt = DateTime.UtcNow
            }
        );

        // SaveChanges here so User IDs are generated before Transactions reference them
        context.SaveChanges();
    }

    // -------------------------------------------------------
    // CATEGORIES
    // -------------------------------------------------------
    private static void SeedCategories(DbContext context)
    {
        if (context.Set<Category>().Any()) return;

        context.Set<Category>().AddRange(
            new Category
            {
                Name = "Laptops",
                Description = "Portable computing devices for business and creative work"
            },
            new Category
            {
                Name = "Monitors",
                Description = "Desktop displays ranging from 24\" to ultrawide formats"
            },
            new Category
            {
                Name = "Networking",
                Description = "Routers, switches, access points, and cabling infrastructure"
            }
        );

        context.SaveChanges();
    }

    // -------------------------------------------------------
    // BRANDS
    // -------------------------------------------------------
    private static void SeedBrands(DbContext context)
    {
        if (context.Set<Brand>().Any()) return;

        context.Set<Brand>().AddRange(
            new Brand
            {
                Name = "Dell",
                ContactEmail = "partnerops@dell.com",
                Website = "https://www.dell.com"
            },
            new Brand
            {
                Name = "LG Electronics",
                ContactEmail = "b2b@lge.com",
                Website = "https://www.lg.com"
            },
            new Brand
            {
                Name = "Cisco",
                ContactEmail = "channelsales@cisco.com",
                Website = "https://www.cisco.com"
            }
        );

        context.SaveChanges();
    }

    // -------------------------------------------------------
    // PRODUCTS
    // -------------------------------------------------------
    private static void SeedProducts(DbContext context)
    {
        if (context.Set<Product>().Any()) return;

        // Resolve IDs from the rows we just inserted
        var laptopCatId = context.Set<Category>().Single(c => c.Name == "Laptops").Id;
        var monitorCatId = context.Set<Category>().Single(c => c.Name == "Monitors").Id;
        var networkCatId = context.Set<Category>().Single(c => c.Name == "Networking").Id;

        var dellId = context.Set<Brand>().Single(b => b.Name == "Dell").Id;
        var lgId = context.Set<Brand>().Single(b => b.Name == "LG Electronics").Id;
        var ciscoId = context.Set<Brand>().Single(b => b.Name == "Cisco").Id;

        var now = DateTime.UtcNow;

        context.Set<Product>().AddRange(

            // --- Laptops ---
            new Product
            {
                SKU = "DL-LAT5540-001",
                Name = "Dell Latitude 5540 15\" Business Laptop",
                Description = "Intel Core i5-1345U, 16GB DDR4, 512GB SSD, Windows 11 Pro",
                CategoryId = laptopCatId,
                BrandId = dellId,
                UnitCost = 849.00m,
                SellPrice = 1099.00m,
                QuantityOnHand = 24,
                ReorderThreshold = 5,
                WarehouseLocation = "A-01-03",
                CreatedAt = now,
                UpdatedAt = now
            },
            new Product
            {
                SKU = "DL-LAT5540-002",
                Name = "Dell Latitude 5540 15\" Business Laptop — i7",
                Description = "Intel Core i7-1365U, 32GB DDR4, 1TB SSD, Windows 11 Pro",
                CategoryId = laptopCatId,
                BrandId = dellId,
                UnitCost = 1149.00m,
                SellPrice = 1499.00m,
                QuantityOnHand = 12,
                ReorderThreshold = 3,
                WarehouseLocation = "A-01-04",
                CreatedAt = now,
                UpdatedAt = now
            },
            new Product
            {
                SKU = "DL-INS3530-001",
                Name = "Dell Inspiron 3530 15\" Entry Laptop",
                Description = "Intel Core i3-1305U, 8GB DDR4, 256GB SSD, Windows 11 Home",
                CategoryId = laptopCatId,
                BrandId = dellId,
                UnitCost = 449.00m,
                SellPrice = 599.00m,
                QuantityOnHand = 3,            // Below reorder threshold — will trigger low-stock alert
                ReorderThreshold = 5,
                WarehouseLocation = "A-01-05",
                CreatedAt = now,
                UpdatedAt = now
            },

            // --- Monitors ---
            new Product
            {
                SKU = "LG-27BN65Q-001",
                Name = "LG 27\" QHD IPS Monitor",
                Description = "2560x1440, 75Hz, IPS panel, USB-C, Height Adjustable Stand",
                CategoryId = monitorCatId,
                BrandId = lgId,
                UnitCost = 219.00m,
                SellPrice = 299.00m,
                QuantityOnHand = 30,
                ReorderThreshold = 8,
                WarehouseLocation = "B-02-01",
                CreatedAt = now,
                UpdatedAt = now
            },
            new Product
            {
                SKU = "LG-34WN80C-001",
                Name = "LG 34\" Ultrawide Curved Monitor",
                Description = "3440x1440, 60Hz, IPS, USB-C 94W PD, Picture-by-Picture",
                CategoryId = monitorCatId,
                BrandId = lgId,
                UnitCost = 449.00m,
                SellPrice = 599.00m,
                QuantityOnHand = 14,
                ReorderThreshold = 4,
                WarehouseLocation = "B-02-02",
                CreatedAt = now,
                UpdatedAt = now
            },
            new Product
            {
                SKU = "LG-24MK430H-001",
                Name = "LG 24\" FHD IPS Monitor",
                Description = "1920x1080, 75Hz, IPS, AMD FreeSync, HDMI x2",
                CategoryId = monitorCatId,
                BrandId = lgId,
                UnitCost = 109.00m,
                SellPrice = 149.00m,
                QuantityOnHand = 0,            // Out of stock
                ReorderThreshold = 10,
                WarehouseLocation = "B-02-03",
                CreatedAt = now,
                UpdatedAt = now
            },

            // --- Networking ---
            new Product
            {
                SKU = "CS-C9200L-24P",
                Name = "Cisco Catalyst 9200L 24-Port PoE Switch",
                Description = "24x GbE PoE+, 4x 1G SFP Uplinks, Network Essentials IOS",
                CategoryId = networkCatId,
                BrandId = ciscoId,
                UnitCost = 1849.00m,
                SellPrice = 2399.00m,
                QuantityOnHand = 8,
                ReorderThreshold = 2,
                WarehouseLocation = "C-03-01",
                CreatedAt = now,
                UpdatedAt = now
            },
            new Product
            {
                SKU = "CS-RV345-K9",
                Name = "Cisco RV345 Dual WAN Gigabit VPN Router",
                Description = "16x GbE LAN, 2x WAN, 100 IPSec VPN tunnels, Web Filtering",
                CategoryId = networkCatId,
                BrandId = ciscoId,
                UnitCost = 279.00m,
                SellPrice = 369.00m,
                QuantityOnHand = 2,            // Below reorder threshold
                ReorderThreshold = 4,
                WarehouseLocation = "C-03-02",
                CreatedAt = now,
                UpdatedAt = now
            },
            new Product
            {
                SKU = "CS-WAP571-K9",
                Name = "Cisco WAP571 Dual Radio 802.11ac Access Point",
                Description = "Wave 2, 4x4 MU-MIMO, PoE, Captive Portal, VLAN support",
                CategoryId = networkCatId,
                BrandId = ciscoId,
                UnitCost = 319.00m,
                SellPrice = 429.00m,
                QuantityOnHand = 18,
                ReorderThreshold = 5,
                WarehouseLocation = "C-03-03",
                CreatedAt = now,
                UpdatedAt = now
            },
            new Product
            {
                SKU = "CS-SF110D-16HP",
                Name = "Cisco SF110D-16HP 16-Port PoE Desktop Switch",
                Description = "16x 10/100 PoE ports, 64W budget, fanless, unmanaged",
                CategoryId = networkCatId,
                BrandId = ciscoId,
                UnitCost = 129.00m,
                SellPrice = 179.00m,
                QuantityOnHand = 22,
                ReorderThreshold = 6,
                WarehouseLocation = "C-03-04",
                CreatedAt = now,
                UpdatedAt = now
            }
        );

        context.SaveChanges();
    }

    // -------------------------------------------------------
    // TRANSACTIONS
    // Seed realistic initial stock receipts for each product
    // -------------------------------------------------------
    private static void SeedTransactions(DbContext context)
    {
        if (context.Set<Transaction>().Any()) return;

        var adminId = context.Set<User>().Single(u => u.Role == "Admin").Id;

        // Fetch all products as a dictionary for easy lookup by SKU
        var products = context.Set<Product>()
            .ToDictionary(p => p.SKU, p => p.Id);

        var now = DateTime.UtcNow;

        context.Set<Transaction>().AddRange(

            // Initial receipt for each product (stock that was here "day one")
            new Transaction
            {
                ProductId = products["DL-LAT5540-001"],
                UserId = adminId,
                Type = TransactionType.Receipt,
                Quantity = 24,
                Notes = "Initial stock receipt — NovaDist warehouse opening",
                CreatedAt = now.AddDays(-30)
            },
            new Transaction
            {
                ProductId = products["DL-LAT5540-002"],
                UserId = adminId,
                Type = TransactionType.Receipt,
                Quantity = 15,
                Notes = "Initial stock receipt — NovaDist warehouse opening",
                CreatedAt = now.AddDays(-30)
            },
            new Transaction
            {
                ProductId = products["DL-INS3530-001"],
                UserId = adminId,
                Type = TransactionType.Receipt,
                Quantity = 10,
                Notes = "Initial stock receipt — NovaDist warehouse opening",
                CreatedAt = now.AddDays(-30)
            },
            // Outgoing transaction — shows the Inspiron running low
            new Transaction
            {
                ProductId = products["DL-INS3530-001"],
                UserId = adminId,
                Type = TransactionType.Outgoing,
                Quantity = 7,
                Notes = "Fulfilled order #1042 — SMB client batch",
                CreatedAt = now.AddDays(-5)
            },
            new Transaction
            {
                ProductId = products["LG-27BN65Q-001"],
                UserId = adminId,
                Type = TransactionType.Receipt,
                Quantity = 30,
                Notes = "Initial stock receipt — NovaDist warehouse opening",
                CreatedAt = now.AddDays(-30)
            },
            new Transaction
            {
                ProductId = products["LG-34WN80C-001"],
                UserId = adminId,
                Type = TransactionType.Receipt,
                Quantity = 14,
                Notes = "Initial stock receipt — NovaDist warehouse opening",
                CreatedAt = now.AddDays(-30)
            },
            new Transaction
            {
                ProductId = products["LG-24MK430H-001"],
                UserId = adminId,
                Type = TransactionType.Receipt,
                Quantity = 20,
                Notes = "Initial stock receipt — NovaDist warehouse opening",
                CreatedAt = now.AddDays(-30)
            },
            // Outgoing — wipes LG 24" to zero
            new Transaction
            {
                ProductId = products["LG-24MK430H-001"],
                UserId = adminId,
                Type = TransactionType.Outgoing,
                Quantity = 20,
                Notes = "Fulfilled order #1055 — enterprise desktop refresh",
                CreatedAt = now.AddDays(-2)
            },
            new Transaction
            {
                ProductId = products["CS-C9200L-24P"],
                UserId = adminId,
                Type = TransactionType.Receipt,
                Quantity = 8,
                Notes = "Initial stock receipt — NovaDist warehouse opening",
                CreatedAt = now.AddDays(-30)
            },
            new Transaction
            {
                ProductId = products["CS-RV345-K9"],
                UserId = adminId,
                Type = TransactionType.Receipt,
                Quantity = 10,
                Notes = "Initial stock receipt — NovaDist warehouse opening",
                CreatedAt = now.AddDays(-30)
            },
            // Outgoing — drops Cisco router below threshold
            new Transaction
            {
                ProductId = products["CS-RV345-K9"],
                UserId = adminId,
                Type = TransactionType.Outgoing,
                Quantity = 8,
                Notes = "Fulfilled order #1061 — retail chain rollout",
                CreatedAt = now.AddDays(-1)
            },
            new Transaction
            {
                ProductId = products["CS-WAP571-K9"],
                UserId = adminId,
                Type = TransactionType.Receipt,
                Quantity = 18,
                Notes = "Initial stock receipt — NovaDist warehouse opening",
                CreatedAt = now.AddDays(-30)
            },
            new Transaction
            {
                ProductId = products["CS-SF110D-16HP"],
                UserId = adminId,
                Type = TransactionType.Receipt,
                Quantity = 25,
                Notes = "Initial stock receipt — NovaDist warehouse opening",
                CreatedAt = now.AddDays(-30)
            },
            // Adjustment — damaged units written off
            new Transaction
            {
                ProductId = products["CS-SF110D-16HP"],
                UserId = adminId,
                Type = TransactionType.Adjustment,
                Quantity = -3,
                Notes = "3 units damaged in transit — written off per ops report #22",
                CreatedAt = now.AddDays(-10)
            }
        );

        // No SaveChanges here — the parent Seed/SeedAsync methods call it once at the end
    }
}