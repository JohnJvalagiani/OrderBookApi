﻿using FluentValidation;
using OrderBook.API.Models.CommandModels;

namespace OrderBook.API.Validators
{
    public class UpdateOrderValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.")
                .MaximumLength(100).WithMessage("UserId cannot exceed 100 characters.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(x => x.OrderType)
                .NotNull().WithMessage("OrderType is required");
        }
    }

}
