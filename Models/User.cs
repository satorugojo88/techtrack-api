namespace TechTrack.API.Models;

public enum AuthRole
{
    Admin,
    Staff
}

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public AuthRole Role { get; set; } = AuthRole.Staff;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property - one User logs many Transactions
    public ICollection<Transaction> Transactions { get; set; } = [];
}
