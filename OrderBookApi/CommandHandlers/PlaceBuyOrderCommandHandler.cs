using Application.Interfaces;
using FluentValidation;
using OrderBook.API.DTOs;
using OrderBook.API.Models.CommandModels;
using System.ComponentModel.DataAnnotations;

namespace OrderBook.API.CommandHandlers
{
    public class PlaceBuyOrderCommandHandler : ICommandHandler<PlaceBuyOrderCommand>
    {
        private readonly IValidator<PlaceBuyOrderCommand> _validator;
        private readonly IOrderBookService _orderBookService;

        public PlaceBuyOrderCommandHandler(IOrderBookService orderBookService, IValidator<PlaceBuyOrderCommand> validator)
        {
            _validator = validator;
            _orderBookService = orderBookService;
        }

        public void Handle(PlaceBuyOrderCommand command)
        {
            var validationResult =  _validator.ValidateAsync(command).Result;

            if (!validationResult.IsValid)
            {
                throw new FluentValidation.ValidationException(validationResult.Errors);
            }

            _orderBookService.PlaceBuyOrder(new WriteOrderDto
            {
                UserId = command.UserId,
                Quantity = command.Quantity,
                Price = command.Price
            });
        }
    }
}
