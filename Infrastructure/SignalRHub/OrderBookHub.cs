using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Infrastructure.SignalRHub
{
  
    public class OrderBookHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", "");
        }
    }

}
