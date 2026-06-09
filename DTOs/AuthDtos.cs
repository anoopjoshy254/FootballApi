using System.ComponentModel.DataAnnotations;

namespace FootballApi.DTOs;

public class RegisterRequestDto
{
    [Required]
    [MaxLength(50)]
    public required string Name { get; set; }

    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [MinLength(6)]
    public required string Password { get; set; }
}

public class LoginRequestDto
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    public required string Password { get; set; }
}

public class AuthResponseDto
{
    public required string Token { get; set; }
}
