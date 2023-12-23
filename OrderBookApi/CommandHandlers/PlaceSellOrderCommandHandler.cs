using Application.Interfaces;
using OrderBook.API.Models.CommandModels;

namespace OrderBook.API.CommandHandlers
{
    public class PlaceSellOrderCommandHandler : ICommandHandler<PlaceSellOrderCommand>
    {
        private readonly IOrderBookService _orderBookService;

        public PlaceSellOrderCommandHandler(IOrderBookService orderBookService)
        {
            _orderBookService = orderBookService;
        }

        public void Handle(PlaceSellOrderCommand command)
        {
            _orderBookService.PlaceSellOrder(new Order
            {
                UserId = command.UserId,
                Quantity = command.Quantity,
                Price = command.Price
            });
        }
    }
}
