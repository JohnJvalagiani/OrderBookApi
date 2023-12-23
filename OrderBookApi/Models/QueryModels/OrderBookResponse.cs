
using OrderBook.API.DTOs;

namespace OrderBook.API.Models.QueryModels
{
        public class OrderBookResponse
        {
            public List<OrderDto> BuyOrders { get; set; }
            public List<OrderDto> SellOrders { get; set; }
        }
}
