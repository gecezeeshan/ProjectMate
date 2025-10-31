
using System.ComponentModel.DataAnnotations;

namespace MiniProjectManager.Api.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, EmailAddress, MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    public List<Project> Projects { get; set; } = new();
}
