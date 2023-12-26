using Application.Profiles;
using Infrastructure.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OrderBook.API.SignalRHub;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<OrderBookContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("OrderBookConnection"))
        );

builder.Services.AddSignalR();
builder.Services.AddAutoMapper(typeof(OrderProfile).Assembly);

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHub<OrderBookHub>("/orderbookhub");

app.MapGet("/", () => "Hello World!");

await app.RunAsync();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
