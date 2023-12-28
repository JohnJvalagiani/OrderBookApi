using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.SignalR;
using OrderBook.API.Models.QueryModels;
using System.Diagnostics;
using System.Threading.Tasks;

namespace OrderBook.API.SignalRHub
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
