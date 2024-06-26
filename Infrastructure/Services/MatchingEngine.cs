﻿using Domain;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class MatchingEngine(OrderBookContext dbContext)
    {
        private readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        public async Task MatchOrders()
        {
            await semaphoreSlim.WaitAsync();

             try
             {

             var buyOrdersTask = dbContext.Orders
             .Where(o => o.OrderType == OrderType.Buy)
             .ToList();

                var sellOrdersTask = dbContext.Orders
                    .Where(o => o.OrderType == OrderType.Sell)
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
            catch(Exception)
            {
                throw;
            }
            finally
            {
                semaphoreSlim.Release();
            }

        }

        private async Task ExecuteTrade(Order buyOrder, Order sellOrder)
        {
            decimal tradeQuantity = Math.Min(buyOrder.Quantity, sellOrder.Quantity);

            buyOrder.Quantity -= tradeQuantity;
            sellOrder.Quantity -= tradeQuantity;

            if (buyOrder.Quantity == 0)
            {
                dbContext.Orders.Remove(buyOrder);
            }

            if (sellOrder.Quantity == 0)
            {
                dbContext.Orders.Remove(sellOrder);
            }

            await dbContext.SaveChangesAsync();
        }
    }

}
