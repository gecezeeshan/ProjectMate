
using System.ComponentModel.DataAnnotations;

namespace MiniProjectManager.Api.DTOs;

public class TaskCreateDto
{
    [Required]
    public string Title { get; set; } = string.Empty;
    public DateTime? DueDate { get; set; }
}

public class TaskUpdateDto
{
    [Required]
    public string Title { get; set; } = string.Empty;
    public DateTime? DueDate { get; set; }
    public bool IsCompleted { get; set; }
}
