using Application.Interfaces;
using OrderBook.API.Models.CommandModels;

namespace OrderBook.API.CommandHandlers
{
    public class PlaceBuyOrderCommandHandler : ICommandHandler<PlaceBuyOrderCommand>
    {
        private readonly IOrderBookService _orderBookService;

        public PlaceBuyOrderCommandHandler(IOrderBookService orderBookService)
        {
            _orderBookService = orderBookService;
        }

        public void Handle(PlaceBuyOrderCommand command)
        {
            _orderBookService.PlaceBuyOrder(new Order
            {
                UserId = command.UserId,
                Quantity = command.Quantity,
                Price = command.Price
            });
        }
    }
}
