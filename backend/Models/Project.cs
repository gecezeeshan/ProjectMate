
using System.ComponentModel.DataAnnotations;

namespace MiniProjectManager.Api.Models;

public class Project
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MinLength(3), MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid OwnerId { get; set; }
    public User? Owner { get; set; }

    public List<TaskItem> Tasks { get; set; } = new();
}
