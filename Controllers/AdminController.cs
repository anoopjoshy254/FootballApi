using FootballApi.DTOs;
using FootballApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FootballApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpPost("teams")]
    public async Task<IActionResult> AddTeam(CreateTeamDto request)
    {
        var result = await _adminService.AddTeamAsync(request);
        return Ok(result);
    }

    [HttpPost("reveal-results")]
    public async Task<IActionResult> RevealResults()
    {
        await _adminService.RevealResultsAsync();
        return Ok(new { Message = "Results have been successfully revealed." });
    }

    [HttpPost("reset-poll")]
    public async Task<IActionResult> ResetPoll()
    {
        await _adminService.ResetPollAsync();
        return Ok(new { Message = "Poll has been successfully reset." });
    }

    [HttpGet("votes")]
    public async Task<IActionResult> GetVoteDetails()
    {
        var result = await _adminService.GetVoteDetailsAsync();
        return Ok(result);
    }

    [HttpGet("results")]
    public async Task<IActionResult> GetResults()
    {
        var result = await _adminService.GetResultsAsync();
        return Ok(result);
    }
}
