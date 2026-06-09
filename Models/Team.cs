namespace FootballApi.Models;

public class Team
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string CountryCode { get; set; }
}
