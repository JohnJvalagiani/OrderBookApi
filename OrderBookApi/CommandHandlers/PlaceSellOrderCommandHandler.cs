using Application.Interfaces;
using FluentValidation;
using OrderBook.API.DTOs;
using OrderBook.API.Models.CommandModels;

namespace OrderBook.API.CommandHandlers
{
    public class PlaceSellOrderCommandHandler : ICommandHandler<PlaceSellOrderCommand>
    {
        private readonly IValidator<PlaceBuyOrderCommand> _validator;

        private readonly IOrderBookService _orderBookService;

        public PlaceSellOrderCommandHandler(IOrderBookService orderBookService, IValidator<PlaceBuyOrderCommand> validator)
        {
            _validator = validator;
            _orderBookService = orderBookService;
        }

        public void Handle(PlaceSellOrderCommand command)
        {
            var validationResult = _validator.ValidateAsync(command).Result;

            if (!validationResult.IsValid)
            {
                throw new FluentValidation.ValidationException(validationResult.Errors);
            }

            _orderBookService.PlaceSellOrder(new WriteOrderDto
            {
                UserId = command.UserId,
                Quantity = command.Quantity,
                Price = command.Price
            });
        }
    }
}
