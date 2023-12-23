using Microsoft.AspNetCore.SignalR;
using OrderBook.API.Models.QueryModels;
using System.Threading.Tasks;

namespace OrderBook.API.SignalRHub
{
  
    public class OrderBookHub : Hub
    {
        public async Task SendOrderBookUpdate(OrderBookResponse orderBook)
        {
            await Clients.All.SendAsync("ReceiveOrderBookUpdate", orderBook);
        }
    }

}
