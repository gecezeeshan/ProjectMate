
using System.ComponentModel.DataAnnotations;

namespace MiniProjectManager.Api.DTOs;

public class ProjectCreateDto
{
    [Required, MinLength(3), MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }
}

public class ProjectDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
}
