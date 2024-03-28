using Application.Interfaces;
using FluentValidation;
using OrderBook.API.Models.CommandModels;

namespace OrderBook.Application.Features.CommandHandlers
{
    public class DeleteOrderCommandHandler : ICommandHandler<DeleteOrderCommand>
    {
        private readonly IOrderBookService _orderBookService;

        public DeleteOrderCommandHandler(IOrderBookService orderBookService)
        {
            _orderBookService = orderBookService;
        }

        public void Handle(DeleteOrderCommand command)
        {
            if (command.OrderId==0)
                throw new ArgumentException("Provided order ID is empty.", nameof(command.OrderId));

            _orderBookService.DeleteOrder(command.OrderId);
        }
    }
}
