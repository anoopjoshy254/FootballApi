using System.ComponentModel.DataAnnotations;

namespace FootballApi.DTOs;

public class SubmitPollDto
{
    [Required]
    public Guid TeamId { get; set; }
}

public class PollResultDto
{
    public required string TeamName { get; set; }
    public int VoteCount { get; set; }
}

public class VoteDetailDto
{
    public required string UserName { get; set; }
    public required string TeamName { get; set; }
    public DateTime VotedAt { get; set; }
}
