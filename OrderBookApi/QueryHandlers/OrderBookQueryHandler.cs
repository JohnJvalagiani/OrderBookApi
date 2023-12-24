using Application.Interfaces;
using OrderBook.API.Models.QueryModels;

namespace OrderBook.API.QueryHandlers
{
    public class OrderBookQueryHandler : IQueryHandler<OrderBookQuery, OrderBookResponse>
    {
        private readonly IOrderBookService _orderBookService;

        public OrderBookQueryHandler(IOrderBookService orderBookService)
        {
            _orderBookService = orderBookService;
        }

        public  OrderBookResponse HandleAsync(OrderBookQuery query)
        {
            var orderBook =  _orderBookService.GetOrderBook().Result;
            return new OrderBookResponse
            {
                BuyOrders = orderBook.BuyOrders,
                SellOrders = orderBook.SellOrders
            };
        }
    }
}
