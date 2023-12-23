using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IOrderBookService
    {
       public OrderBook GetOrderBook();
       public void PlaceBuyOrder(Order order);
       public void PlaceSellOrder(Order order);
       public void UpdateOrder(int orderId, Order updatedOrder);
       public void DeleteOrder(int orderId);
    }
}
