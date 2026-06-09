using System.Security.Claims;
using FootballApi.DTOs;
using FootballApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FootballApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("teams")]
    [Authorize(Roles = "User,Admin")]
    public async Task<IActionResult> GetTeams()
    {
        var result = await _userService.GetTeamsAsync();
        return Ok(result);
    }

    [HttpPost("poll")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> SubmitPoll(SubmitPollDto request)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdString, out Guid userId))
        {
            return Unauthorized(new { Message = "Invalid user token." });
        }

        try
        {
            await _userService.SubmitPollAsync(userId, request);
            return Ok(new { Message = "Vote submitted successfully." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpGet("results")]
    [Authorize(Roles = "User,Admin")]
    public async Task<IActionResult> GetResults()
    {
        try
        {
            var result = await _userService.GetResultsAsync();
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { Message = ex.Message });
        }
    }
}
