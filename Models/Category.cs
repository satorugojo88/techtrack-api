namespace TechTrack.API.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    // Navigation property - one Category has many Products
    public ICollection<Product> Products { get; set; } = [];
}
