using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace SignalRTest.Hubs
{
    public class MyHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            Globals.SystemLogger.LogTrace($"Starting onConnectedAsync for user with connectionId {Context.ConnectionId}.");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception e)
        {
            Globals.SystemLogger.LogTrace($"Starting onDisconnectedAsync for user with connectionId {Context.ConnectionId}.");
            return base.OnDisconnectedAsync(e);
        }
    }
}
