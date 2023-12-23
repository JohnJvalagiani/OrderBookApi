using Application.Interfaces;
using Entities;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class OrderBookService : IOrderBookService
    {
        private readonly OrderBookContext _dbContext;

        public OrderBookService(OrderBookContext dbContext)
        {
            _dbContext = dbContext;
        }

        public OrderBook GetOrderBook()
        {
            return new OrderBook
            {
                BuyOrders = _dbContext.Orders.Where(o => o.OrderType == OrderType.Buy).ToList(),
                SellOrders = _dbContext.Orders.Where(o => o.OrderType == OrderType.Sell).ToList()
            };
        }

        public void PlaceBuyOrder(Order order)
        {
            order.OrderType = OrderType.Buy;
            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();
        }

        public void PlaceSellOrder(Order order)
        {
            order.OrderType = OrderType.Sell;
            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();
        }

        public void UpdateOrder(int orderId, Order updatedOrder)
        {
            var orderToUpdate = _dbContext.Orders.FirstOrDefault(o => o.OrderId == orderId);

            if (orderToUpdate != null)
            {
                // Update order properties as needed
                orderToUpdate.Quantity = updatedOrder.Quantity;
                orderToUpdate.Price = updatedOrder.Price;

                _dbContext.SaveChanges();
            }
        }

        public void DeleteOrder(int orderId)
        {
            var orderToDelete = _dbContext.Orders.FirstOrDefault(o => o.OrderId == orderId);

            if (orderToDelete != null)
            {
                _dbContext.Orders.Remove(orderToDelete);
                _dbContext.SaveChanges();
            }
        }
    }

}

