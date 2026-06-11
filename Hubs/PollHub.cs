using Microsoft.AspNetCore.SignalR;

namespace FootballApi.Hubs;

public class PollHub : Hub
{
    // The client will listen for "ReceivePollUpdate"
    // No specific server-to-client methods need to be defined here
    // since we use Clients.All.SendAsync() dynamically, but it's good practice.
}
