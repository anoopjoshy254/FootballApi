using FootballApi.DTOs;

namespace FootballApi.Services;

public interface IUserService
{
    Task<List<TeamResponseDto>> GetTeamsAsync();
    Task SubmitPollAsync(Guid userId, SubmitPollDto request);
    Task<List<PollResultDto>> GetResultsAsync();
}
