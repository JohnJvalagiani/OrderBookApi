using Application.Interfaces;
using Application.Models;
using FluentValidation;
using OrderBook.API.Models.CommandModels;

namespace OrderBook.Application.Features.CommandHandlers
{
    public class UpdateOrderCommandHandler : ICommandHandler<UpdateOrderCommand>
    {
        private readonly IValidator<UpdateOrderCommand> _validator;

        private readonly IOrderBookService _orderBookService;

        public UpdateOrderCommandHandler(IOrderBookService orderBookService, IValidator<UpdateOrderCommand> validator)
        {
            _validator = validator;
            _orderBookService = orderBookService;
        }

        public void Handle(UpdateOrderCommand command)
        {
            var validationResult = _validator.ValidateAsync(command).Result;

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            _orderBookService.UpdateOrder(command.OrderId,new WriteOrderDto
            {
                UserId = command.UserId,
                Quantity = command.Quantity,
                Price = command.Price,
                OrderType = command.OrderType,
            });
        }
    }
}
