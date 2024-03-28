using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OrderBook.API.Models.CommandModels;
using OrderBook.API.Models.QueryModels;
using OrderBook.API.QueryHandlers;
using Newtonsoft.Json;
using Domain;
using MassTransit.Mediator;
using Infrastructure.SignalRHub;

[Route("api/[controller]")]
[ApiController]
public class OrderBookController(IMediator mediator, IHubContext<OrderBookHub> orderBookHubContext) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<OrderBookResponse>> GetOrderBook()
    {
           var orderBook = mediator.Send(new OrderBookQuery());
            return Ok(orderBook);
    }

    [HttpPost("buy")]
    public ActionResult PlaceBuyOrder(PlaceBuyOrderCommand command)
    {
            mediator.Send(command);
            NotifyOrderBookUpdate();
            return Ok();
    }

    [HttpPost("sell")]
    public ActionResult PlaceSellOrder(PlaceSellOrderCommand command)
    {
            mediator.Send(command);
            NotifyOrderBookUpdate();
            return Ok();
    }

    [HttpPut("update")]
    public ActionResult UpdateOrder(UpdateOrderCommand command)
    {
            mediator.Send(command);
            NotifyOrderBookUpdate();
            return Ok();
    }

    [HttpDelete("delete/{orderId}")]
    public ActionResult DeleteOrder(int orderId)
    {
            mediator.Send(new DeleteOrderCommand { OrderId = orderId });
            NotifyOrderBookUpdate();
            return Ok();
    }

    private async Task NotifyOrderBookUpdate()
    {
        var orderBook = mediator.Send(new OrderBookQuery());
        var OrdersJson = JsonConvert.SerializeObject(orderBook);
        await orderBookHubContext.Clients.All.SendAsync("ReceiveMessage", OrdersJson);
    }
}
