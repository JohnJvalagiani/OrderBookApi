using DTOs;
using Entities;
using OrderBook.API.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IOrderBookService
    {
       public Task<OrderBookModel> GetOrderBook();
       public Task<ReadOrderDto> PlaceBuyOrder(WriteOrderDto order);
       public Task<ReadOrderDto> PlaceSellOrder(WriteOrderDto order);
       public Task<WriteOrderDto> UpdateOrder(int orderId, WriteOrderDto updatedOrder);
       public Task<bool> DeleteOrder(int orderId);
    }
}
