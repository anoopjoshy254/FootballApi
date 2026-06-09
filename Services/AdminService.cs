using FootballApi.Data;
using FootballApi.DTOs;
using FootballApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballApi.Services;

public class AdminService : IAdminService
{
    private readonly AppDbContext _context;

    public AdminService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TeamResponseDto> AddTeamAsync(CreateTeamDto request)
    {
        var team = new Team
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            CountryCode = request.CountryCode
        };

        _context.Teams.Add(team);
        await _context.SaveChangesAsync();

        return new TeamResponseDto
        {
            Id = team.Id,
            Name = team.Name,
            CountryCode = team.CountryCode
        };
    }

    public async Task RevealResultsAsync()
    {
        var state = await _context.TournamentStates.FirstAsync();
        state.AreResultsRevealed = true;
        await _context.SaveChangesAsync();
    }

    public async Task ResetPollAsync()
    {
        // Delete all votes
        _context.PollVotes.RemoveRange(_context.PollVotes);
        
        // Hide results
        var state = await _context.TournamentStates.FirstAsync();
        state.AreResultsRevealed = false;
        
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<VoteDetailDto>> GetVoteDetailsAsync()
    {
        return await _context.PollVotes
            .Include(v => v.User)
            .Include(v => v.Team)
            .Select(v => new VoteDetailDto
            {
                UserName = v.User.Name,
                TeamName = v.Team.Name,
                VotedAt = v.VotedAt
            })
            .OrderByDescending(v => v.VotedAt)
            .ToListAsync();
    }

    public async Task<List<PollResultDto>> GetResultsAsync()
    {
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
