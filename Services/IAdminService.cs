using FootballApi.DTOs;

namespace FootballApi.Services;

public interface IAdminService
{
    Task<TeamResponseDto> AddTeamAsync(CreateTeamDto request);
    Task RevealResultsAsync();
    Task ResetPollAsync();
    Task<IEnumerable<VoteDetailDto>> GetVoteDetailsAsync();
    Task<List<PollResultDto>> GetResultsAsync();
}
