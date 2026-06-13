namespace TechTrack.API.Models;

public enum TransactionType
{
    Receipt,
    Adjustment,
    Outgoing
}


public class Transaction
{
    public int Id { get; set; }

    // Foreign keys
    public int ProductId { get; set; }
    public int UserId { get; set; }


    public TransactionType Type { get; set; }
    public int Quantity { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public Product Product { get; set; } = null!;
    public User User { get; set; } = null!;
}
