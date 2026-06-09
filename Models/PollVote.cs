namespace FootballApi.Models;

public class PollVote
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid TeamId { get; set; }
    public Team Team { get; set; } = null!;
    public DateTime VotedAt { get; set; }
}
