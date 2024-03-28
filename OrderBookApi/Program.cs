using Application.Profiles;
using Infrastructure.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OrderBook.API.Middleware;
using OrderBook.API.QueryHandlers;
using Application.Interfaces;
using Infrastructure.Services;
using Infrastructure.SignalRHub;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<OrderBookContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("OrderBookConnection"))
        );

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddScoped<IOrderBookService, OrderBookService>();
builder.Services.AddScoped<MatchingEngine>();

builder.Services.AddSignalR();
builder.Services.AddAutoMapper(typeof(OrderProfile).Assembly);


var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHub<OrderBookHub>("orderbookhub");

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();



await app.RunAsync();
