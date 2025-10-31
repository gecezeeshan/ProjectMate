
using System.ComponentModel.DataAnnotations;

namespace MiniProjectManager.Api.DTOs;

public class RegisterRequest
{
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [Required, MinLength(2)] public string Name { get; set; } = string.Empty;
    [Required, MinLength(6)] public string Password { get; set; } = string.Empty;
}

public class LoginRequest
{
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
}

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
