using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OrderBook.API.CommandHandlers;
using OrderBook.API.Models.CommandModels;
using OrderBook.API.Models.QueryModels;
using OrderBook.API.QueryHandlers;
using OrderBook.API.SignalRHub;

[ApiController]
[Route("api/orderbook")]
public class OrderBookController : ControllerBase
{
    private readonly ICommandHandler<PlaceBuyOrderCommand> _placeBuyOrderCommandHandler;
    private readonly ICommandHandler<PlaceSellOrderCommand> _placeSellOrderCommandHandler;
    private readonly ICommandHandler<UpdateOrderCommand> _updateOrderCommandHandler;
    private readonly ICommandHandler<DeleteOrderCommand> _deleteOrderCommandHandler;
    private readonly IQueryHandler<OrderBookQuery, OrderBookResponse> _orderBookQueryHandler;
    private readonly IHubContext<OrderBookHub> _orderBookHubContext;

    public OrderBookController(
        ICommandHandler<PlaceBuyOrderCommand> placeBuyOrderCommandHandler,
        ICommandHandler<PlaceSellOrderCommand> placeSellOrderCommandHandler,
        ICommandHandler<UpdateOrderCommand> updateOrderCommandHandler,
        ICommandHandler<DeleteOrderCommand> deleteOrderCommandHandler,
        IQueryHandler<OrderBookQuery, OrderBookResponse> orderBookQueryHandler,
        IHubContext<OrderBookHub> orderBookHubContext)
    {
        _placeBuyOrderCommandHandler = placeBuyOrderCommandHandler;
        _placeSellOrderCommandHandler = placeSellOrderCommandHandler;
        _updateOrderCommandHandler = updateOrderCommandHandler;
        _deleteOrderCommandHandler = deleteOrderCommandHandler;
        _orderBookQueryHandler = orderBookQueryHandler;
        _orderBookHubContext = orderBookHubContext;
    }

    [HttpGet]
    public ActionResult<OrderBookResponse> GetOrderBook()
    {
        try
        {
            var orderBook = _orderBookQueryHandler.Handle(new OrderBookQuery());
            return Ok(orderBook);
        }
        catch (Exception ex)
        {
            // Log the exception
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost("buy")]
    public ActionResult PlaceBuyOrder(PlaceBuyOrderCommand command)
    {
        try
        {
            _placeBuyOrderCommandHandler.Handle(command);
            NotifyOrderBookUpdate();
            return Ok();
        }
        catch (Exception ex)
        {
            // Log the exception
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost("sell")]
    public ActionResult PlaceSellOrder(PlaceSellOrderCommand command)
    {
        try
        {
            _placeSellOrderCommandHandler.Handle(command);
            NotifyOrderBookUpdate();
            return Ok();
        }
        catch (Exception ex)
        {
            // Log the exception
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPut("update")]
    public ActionResult UpdateOrder(UpdateOrderCommand command)
    {
        try
        {
            _updateOrderCommandHandler.Handle(command);
            NotifyOrderBookUpdate();
            return Ok();
        }
        catch (Exception ex)
        {
            // Log the exception
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpDelete("delete/{orderId}")]
    public ActionResult DeleteOrder(string orderId)
    {
        try
        {
            _deleteOrderCommandHandler.Handle(new DeleteOrderCommand { OrderId = orderId });
            NotifyOrderBookUpdate();
            return Ok();
        }
        catch (Exception ex)
        {
            // Log the exception
            return StatusCode(500, "Internal Server Error");
        }
    }

    private async void NotifyOrderBookUpdate()
    {
        var orderBook = _orderBookQueryHandler.Handle(new OrderBookQuery());
        await _orderBookHubContext.Clients.All.SendAsync("ReceiveOrderBookUpdate", orderBook);
    }
}
