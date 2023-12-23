using Entities;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class MatchingEngine
    {
        private readonly OrderBookContext _dbContext;

        public MatchingEngine(OrderBookContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void MatchOrders()
        {
            // Get all buy and sell orders from the database
            var buyOrders = _dbContext.Orders.Where(o => o.OrderType == OrderType.Buy).OrderByDescending(o => o.Price).ThenBy(o => o.CreatedAt).ToList();
            var sellOrders = _dbContext.Orders.Where(o => o.OrderType == OrderType.Sell).OrderBy(o => o.Price).ThenBy(o => o.CreatedAt).ToList();

            // Match buy and sell orders
            foreach (var buyOrder in buyOrders)
            {
                foreach (var sellOrder in sellOrders)
                {
                    if (sellOrder.Price <= buyOrder.Price)
                    {
                        // Match found, execute the trade
                        ExecuteTrade(buyOrder, sellOrder);

                        // Remove matched orders from the lists
                        buyOrders.Remove(buyOrder);
                        sellOrders.Remove(sellOrder);

                        // Continue matching with the next buy order
                        break;
                    }
                }
            }
        }

        private void ExecuteTrade(Order buyOrder, Order sellOrder)
        {
            // Perform trade execution logic, update quantities, etc.
            // For simplicity, this example assumes a complete match, and the entire quantities are traded.

            decimal tradeQuantity = Math.Min(buyOrder.Quantity, sellOrder.Quantity);

            // Update quantities
            buyOrder.Quantity -= tradeQuantity;
            sellOrder.Quantity -= tradeQuantity;

            // Remove orders with zero quantity
            if (buyOrder.Quantity == 0)
            {
                _dbContext.Orders.Remove(buyOrder);
            }

            if (sellOrder.Quantity == 0)
            {
                _dbContext.Orders.Remove(sellOrder);
            }

            // Save changes to the database
            _dbContext.SaveChanges();
        }
    }

}
