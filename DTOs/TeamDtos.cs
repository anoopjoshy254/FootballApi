using System.ComponentModel.DataAnnotations;

namespace FootballApi.DTOs;

public class CreateTeamDto
{
    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }

    [Required]
    [StringLength(3, MinimumLength = 2)]
    public required string CountryCode { get; set; }
}

public class TeamResponseDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string CountryCode { get; set; }
}
