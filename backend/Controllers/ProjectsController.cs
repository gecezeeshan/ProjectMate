
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniProjectManager.Api.Data;
using MiniProjectManager.Api.DTOs;
using MiniProjectManager.Api.Models;

namespace MiniProjectManager.Api.Controllers;

[ApiController]
[Route("api/v1/projects")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly AppDbContext _db;
    public ProjectsController(AppDbContext db) { _db = db; }

    private Guid UserId() =>
        Guid.Parse(User.Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value);

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects()
    {
        var uid = UserId();
        var list = await _db.Projects.AsNoTracking()
            .Where(p => p.OwnerId == uid)
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new ProjectDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                CreatedAt = p.CreatedAt
            }).ToListAsync();

        return Ok(list);
    }

    [HttpPost]
    public async Task<ActionResult<ProjectDto>> Create([FromBody] ProjectCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var project = new Project
        {
            Title = dto.Title,
            Description = dto.Description,
            OwnerId = UserId()
        };
        _db.Projects.Add(project);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = project.Id }, new ProjectDto
        {
            Id = project.Id,
            Title = project.Title,
            Description = project.Description,
            CreatedAt = project.CreatedAt
        });
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Project>> GetById(Guid id)
    {
        var uid = UserId();
        var project = await _db.Projects
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == id && p.OwnerId == uid);
        if (project is null) return NotFound();
        return Ok(project);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var uid = UserId();
        var project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == id && p.OwnerId == uid);
        if (project is null) return NotFound();
        _db.Projects.Remove(project);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // Smart Scheduler endpoint (Assignment 3 enhancement)
    [HttpPost("{projectId:guid}/schedule")]
    public async Task<ActionResult<object>> Schedule(Guid projectId, [FromBody] dynamic? input)
    {
        var uid = UserId();
        var project = await _db.Projects.Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == projectId && p.OwnerId == uid);
        if (project is null) return NotFound();

        // Simple heuristic: order tasks by (IsCompleted asc, DueDate asc nulls last)
        var now = DateTime.UtcNow;
        var ordered = project.Tasks
            .Where(t => !t.IsCompleted)
            .OrderBy(t => t.IsCompleted)
            .ThenBy(t => t.DueDate ?? DateTime.MaxValue)
            .Select((t, idx) => new {
                taskId = t.Id,
                title = t.Title,
                suggestedStart = now.Date.AddDays(idx),
                suggestedEnd = (t.DueDate ?? now.Date.AddDays(idx + 1))
            }).ToList();

        return Ok(new {
            projectId = project.Id,
            count = ordered.Count,
            plan = ordered
        });
    }
}
