namespace TechTrack.API.Models;

public class Brand
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? ContactEmail { get; set; }
    public string? Website { get; set; }

    // Navigation property - one Brand has many Products
    public ICollection<Product> Products { get; set; } = [];

}