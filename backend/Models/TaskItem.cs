
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniProjectManager.Api.Models;

public class TaskItem
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string Title { get; set; } = string.Empty;

    public DateTime? DueDate { get; set; }
    public bool IsCompleted { get; set; } = false;

    public Guid ProjectId { get; set; }
    public Project? Project { get; set; }
}
