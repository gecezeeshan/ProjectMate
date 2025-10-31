
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniProjectManager.Api.Data;
using MiniProjectManager.Api.DTOs;
using MiniProjectManager.Api.Models;
using MiniProjectManager.Api.Services;

namespace MiniProjectManager.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly JwtService _jwt;
    public AuthController(AppDbContext db, JwtService jwt)
    {
        _db = db; _jwt = jwt;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest req)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (await _db.Users.AnyAsync(u => u.Email == req.Email))
            return Conflict("Email already exists");

        var user = new User
        {
            Email = req.Email,
            Name = req.Name,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password)
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        var token = _jwt.Generate(user);
        return Ok(new AuthResponse { Token = token, Email = user.Email, Name = user.Name });
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest req)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == req.Email);
        if (user is null) return Unauthorized("Invalid credentials");
        if (!BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
            return Unauthorized("Invalid credentials");

        var token = _jwt.Generate(user);
        return Ok(new AuthResponse { Token = token, Email = user.Email, Name = user.Name });
    }
}
