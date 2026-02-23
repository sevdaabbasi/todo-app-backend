using System.ComponentModel.DataAnnotations;

namespace TodoApp.Core.DTOs.Auth;

public class SignInDto
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
} 