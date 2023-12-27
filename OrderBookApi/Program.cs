using Application.Profiles;
using Infrastructure.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OrderBook.API.SignalRHub;
using OrderBook.API.Middleware;
using Microsoft.AspNetCore.SignalR;
using OrderBook.API.CommandHandlers;
using OrderBook.API.Models.CommandModels;
using OrderBook.API.Models.QueryModels;
using OrderBook.API.QueryHandlers;
using Application.Interfaces;
using Application.Services;
using FluentValidation;
using System;
using OrderBook.API.Validators;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<OrderBookContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("OrderBookConnection"))
        );

builder.Services.AddScoped<IOrderBookService, OrderBookService>();
builder.Services.AddScoped<MatchingEngine>();
builder.Services.AddScoped<ICommandHandler<PlaceBuyOrderCommand>,PlaceBuyOrderCommandHandler>();
builder.Services.AddScoped<ICommandHandler<PlaceSellOrderCommand>, PlaceSellOrderCommandHandler>();
builder.Services.AddScoped<ICommandHandler<UpdateOrderCommand>, UpdateOrderCommandHandler>();
builder.Services.AddScoped<ICommandHandler<DeleteOrderCommand>, DeleteOrderCommandHandler>();
builder.Services.AddScoped<IQueryHandler<OrderBookQuery, OrderBookResponse>, OrderBookQueryHandler>();

builder.Services.AddSignalR();
builder.Services.AddAutoMapper(typeof(OrderProfile).Assembly);

builder.Services.AddScoped<IValidator<PlaceBuyOrderCommand>, BuyOrderValidator>();
builder.Services.AddScoped<IValidator<PlaceSellOrderCommand>, SellOrderValidator>();
builder.Services.AddScoped<IValidator<UpdateOrderCommand>, UpdateOrderValidator>();
//builder.Services.AddSignalR().AddHubOptions<OrderBookHub>(options =>
//{
//});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHub<OrderBookHub>("orderbookhub");

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

//app.UseExceptionMiddleware(); // Move this line after the MapControllers

await app.RunAsync();
