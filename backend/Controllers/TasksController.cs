
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniProjectManager.Api.Data;
using MiniProjectManager.Api.DTOs;
using MiniProjectManager.Api.Models;

namespace MiniProjectManager.Api.Controllers;

[ApiController]
[Route("api/v1/projects/{projectId:guid}/tasks")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _db;
    public TasksController(AppDbContext db) { _db = db; }

    private Guid UserId() =>
        Guid.Parse(User.Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value);

    private async Task<Project?> LoadProject(Guid projectId) =>
        await _db.Projects.Include(p => p.Tasks)
                          .FirstOrDefaultAsync(p => p.Id == projectId && p.OwnerId == UserId());

    [HttpPost]
    public async Task<ActionResult<TaskItem>> Create(Guid projectId, [FromBody] TaskCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var project = await LoadProject(projectId);
        if (project is null) return NotFound();

        var task = new TaskItem { Title = dto.Title, DueDate = dto.DueDate, ProjectId = project.Id };
        _db.Tasks.Add(task);
        await _db.SaveChangesAsync();
        return Ok(task);
    }

    [HttpPut("{taskId:guid}")]
    public async Task<ActionResult<TaskItem>> Update(Guid projectId, Guid taskId, [FromBody] TaskUpdateDto dto)
    {
        var project = await LoadProject(projectId);
        if (project is null) return NotFound();

        var task = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == taskId && t.ProjectId == project.Id);
        if (task is null) return NotFound();

        task.Title = dto.Title;
        task.DueDate = dto.DueDate;
        task.IsCompleted = dto.IsCompleted;
        await _db.SaveChangesAsync();
        return Ok(task);
    }

    [HttpDelete("{taskId:guid}")]
    public async Task<IActionResult> Delete(Guid projectId, Guid taskId)
    {
        var project = await LoadProject(projectId);
        if (project is null) return NotFound();

        var task = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == taskId && t.ProjectId == project.Id);
        if (task is null) return NotFound();

        _db.Tasks.Remove(task);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
