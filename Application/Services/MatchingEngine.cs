using Entities;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
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

        public async Task MatchOrders()
        {
            var buyOrdersTask =  _dbContext.Orders
                .Where(o => o.OrderType == OrderType.Buy)
                .OrderByDescending(o => o.Price)
                .ThenBy(o => o.CreatedAt)
                .ToList();

            var sellOrdersTask =  _dbContext.Orders
                .Where(o => o.OrderType == OrderType.Sell)
                .OrderBy(o => o.Price)
                .ThenBy(o => o.CreatedAt)
                .ToList();

            var buyOrders = buyOrdersTask;
            var sellOrders = sellOrdersTask;

            buyOrders.Sort((a, b) => b.Price.CompareTo(a.Price) != 0 ? b.Price.CompareTo(a.Price) : a.CreatedAt.CompareTo(b.CreatedAt));
            sellOrders.Sort((a, b) => a.Price.CompareTo(b.Price) != 0 ? a.Price.CompareTo(b.Price) : a.CreatedAt.CompareTo(b.CreatedAt));

            int buyIndex = 0;
            int sellIndex = 0;

            while (buyIndex < buyOrders.Count && sellIndex < sellOrders.Count)
            {
                var buyOrder = buyOrders[buyIndex];
                var sellOrder = sellOrders[sellIndex];

                if (sellOrder.Price <= buyOrder.Price)
                {
                    // Match found, execute the trade
                    await ExecuteTrade(buyOrder, sellOrder);

                    // Remove matched orders from the lists
                    buyOrders.RemoveAt(buyIndex);
                    sellOrders.RemoveAt(sellIndex);
                }
                else
                {
                    // No match, move to the next sell order
                    sellIndex++;
                }
            }


        }

        private async Task ExecuteTrade(Order buyOrder, Order sellOrder)
        {
            decimal tradeQuantity = Math.Min(buyOrder.Quantity, sellOrder.Quantity);

            buyOrder.Quantity -= tradeQuantity;
            sellOrder.Quantity -= tradeQuantity;

            if (buyOrder.Quantity == 0)
            {
                _dbContext.Orders.Remove(buyOrder);
            }

            if (sellOrder.Quantity == 0)
            {
                _dbContext.Orders.Remove(sellOrder);
            }

            await _dbContext.SaveChangesAsync();
        }
    }

}
