namespace PasswordGenerator.Api.Models;

public sealed class PasswordRecord
{
    public Guid Id { get; set; }

    public string Password { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
