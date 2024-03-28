

using Application.Models;

namespace OrderBook.API.Models.QueryModels
{
        public class OrderBookResponse
        {
        public IEnumerable<ReadOrderDto> BuyOrders { get; set; }
        public IEnumerable<ReadOrderDto> SellOrders { get; set; }
    }
}
