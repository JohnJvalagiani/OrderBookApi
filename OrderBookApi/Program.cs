using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OrderBook.API.SignalRHub;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<OrderBookContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("OrderBookDatabase"))
        );


var app = builder.Build();

builder.Services.AddSignalR();

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
