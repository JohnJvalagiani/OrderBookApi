
using Application.Models;

namespace Application.Interfaces
{
    public interface IOrderBookService
    {
       public Task<ServiceResponse<OrderBookModel>> GetOrderBook();
       public Task<ServiceResponse<ReadOrderDto>> PlaceBuyOrder(WriteOrderDto order);
       public Task<ServiceResponse<ReadOrderDto>> PlaceSellOrder(WriteOrderDto order);
       public Task<ServiceResponse<WriteOrderDto>> UpdateOrder(int orderId, WriteOrderDto updatedOrder);
       public Task<ServiceResponse<bool>> DeleteOrder(int orderId);
    }
}
