using FootballApi.Data;
using FootballApi.DTOs;
using FootballApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballApi.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<TeamResponseDto>> GetTeamsAsync()
    {
        return await _context.Teams
            .Select(t => new TeamResponseDto
            {
                Id = t.Id,
                Name = t.Name,
                CountryCode = t.CountryCode
            })
            .ToListAsync();
    }

    public async Task SubmitPollAsync(Guid userId, SubmitPollDto request)
    {
        var state = await _context.TournamentStates.FirstAsync();
        if (state.AreResultsRevealed)
        {
            throw new InvalidOperationException("The poll has ended and results are published. You cannot vote anymore.");
        }

        var hasVoted = await _context.PollVotes.AnyAsync(pv => pv.UserId == userId);
        if (hasVoted)
        {
            throw new InvalidOperationException("User has already voted.");
        }

        var teamExists = await _context.Teams.AnyAsync(t => t.Id == request.TeamId);
        if (!teamExists)
        {
            throw new ArgumentException("Team does not exist.");
        }

        var vote = new PollVote
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TeamId = request.TeamId,
            VotedAt = DateTime.UtcNow
        };

        _context.PollVotes.Add(vote);
        await _context.SaveChangesAsync();
    }

    public async Task<List<PollResultDto>> GetResultsAsync()
    {
        var state = await _context.TournamentStates.FirstAsync();
        if (!state.AreResultsRevealed)
        {
            throw new UnauthorizedAccessException("Results are not yet revealed by the Admin.");
        }

        return await _context.PollVotes
            .GroupBy(pv => pv.Team)
            .Select(g => new PollResultDto
            {
                TeamName = g.Key.Name,
                VoteCount = g.Count()
            })
            .ToListAsync();
    }
}
