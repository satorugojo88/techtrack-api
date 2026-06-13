namespace TechTrack.API.Models;

public class Product
{
    public int Id { get; set; }
    public string SKU { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    // Foreign keys
    public int CategoryId { get; set; }
    public int BrandId { get; set; }


    public decimal UnitCost { get; set; }
    public decimal SellPrice { get; set; }
    public int QuantityOnHand { get; set; }
    public int ReorderThreshold { get; set; }
    public string? WarehouseLocation { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }


    // Navigation properties
    public Category Category { get; set; } = null!;
    public Brand Brand { get; set; } = null!;
    public ICollection<Transaction> Transactions { get; set; } = [];
}
