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
            // Log or debug message indicating a client connected
            Debug.WriteLine($"Client connected: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        public async Task SendOrderBookUpdate(OrderBookResponse orderBook)
        {
            var fullUrl = Context.GetHttpContext().Request.GetDisplayUrl();
          
            Debug.WriteLine($"_______________________________________________________________________________");
            await Clients.All.SendAsync("SendOrderBookUpdate", "aah");
        }
    }

}
