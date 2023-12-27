using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OrderBook.API.CommandHandlers;
using OrderBook.API.Models.CommandModels;
using OrderBook.API.Models.QueryModels;
using OrderBook.API.QueryHandlers;
using OrderBook.API.SignalRHub;

[Route("api/[controller]")]
[ApiController]
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
    public async Task<ActionResult<OrderBookResponse>> GetOrderBook()
    {

        var orderBook = _orderBookQueryHandler.HandleAsync(new OrderBookQuery());
        
        await _orderBookHubContext.Clients.All.SendAsync("message", "orderBook");

            return Ok(orderBook);
    }

    [HttpPost("buy")]
    public ActionResult PlaceBuyOrder(PlaceBuyOrderCommand command)
    {
            _placeBuyOrderCommandHandler.Handle(command);
            NotifyOrderBookUpdate();
            return Ok();
    }

    [HttpPost("sell")]
    public ActionResult PlaceSellOrder(PlaceSellOrderCommand command)
    {
            _placeSellOrderCommandHandler.Handle(command);
            NotifyOrderBookUpdate();
            return Ok();
    }

    [HttpPut("update")]
    public ActionResult UpdateOrder(UpdateOrderCommand command)
    {
            _updateOrderCommandHandler.Handle(command);
            NotifyOrderBookUpdate();
            return Ok();
    }

    [HttpDelete("delete/{orderId}")]
    public ActionResult DeleteOrder(int orderId)
    {
            _deleteOrderCommandHandler.Handle(new DeleteOrderCommand { OrderId = orderId });
            NotifyOrderBookUpdate();
            return Ok();
    }

    private async Task NotifyOrderBookUpdate()
    {
        var orderBook =  _orderBookQueryHandler.HandleAsync(new OrderBookQuery());
        await _orderBookHubContext.Clients.All.SendAsync("SendOrderBookUpdate", "aah");
    }
}
