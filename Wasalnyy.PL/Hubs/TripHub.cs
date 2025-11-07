using Microsoft.AspNetCore.SignalR;

namespace Wasalnyy.PL.Hubs
{
    public class TripHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
