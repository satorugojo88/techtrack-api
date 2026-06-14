namespace TechTrack.API.Models;



public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "Staff";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property - one User logs many Transactions
    public ICollection<Transaction> Transactions { get; set; } = [];
}
