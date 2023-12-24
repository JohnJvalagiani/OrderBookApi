using Application.CustomExceptions;
using Application.Interfaces;
using AutoMapper;
using DTOs;
using Entities;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using OrderBook.API.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class OrderBookService : IOrderBookService
    {
        private readonly IMapper _mapper;
        private readonly OrderBookContext _dbContext;
        private readonly MatchingEngine _matchingEngine;

        public OrderBookService(OrderBookContext dbContext, MatchingEngine matchingEngine, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _matchingEngine = matchingEngine;
        }

        public async Task<OrderBookModel> GetOrderBook()
        {
            var buyOrdersTask = _dbContext.Orders
                                          .Where(o => o.OrderType == OrderType.Buy)
                                          .ToListAsync();

            var sellOrdersTask = _dbContext.Orders
                                           .Where(o => o.OrderType == OrderType.Sell)
                                           .ToListAsync();

            await Task.WhenAll(buyOrdersTask, sellOrdersTask);

            var orderBook = new OrderBookModel
            {
                BuyOrders = _mapper.Map<IEnumerable<ReadOrderDto>>(buyOrdersTask.Result),
                SellOrders = _mapper.Map<IEnumerable<ReadOrderDto>>(sellOrdersTask.Result)
            };

            return orderBook;
        }

        public async Task<ReadOrderDto> PlaceBuyOrder(WriteOrderDto order)
        {
            order.OrderType = OrderType.Buy;

            var theOrder = await _dbContext.Orders.AddAsync(_mapper.Map<Order>(order));

            await _dbContext.SaveChangesAsync();

            _matchingEngine.MatchOrders();
            
            return _mapper.Map<ReadOrderDto>(theOrder);
        }

        public async Task<ReadOrderDto> PlaceSellOrder(WriteOrderDto order)
        {
            order.OrderType = OrderType.Sell;

            var theOrder = await _dbContext.Orders.AddAsync(_mapper.Map<Order>(order));

            await _dbContext.SaveChangesAsync();

             _matchingEngine.MatchOrders();

            return _mapper.Map<ReadOrderDto>(theOrder);
        }

        public async Task<WriteOrderDto> UpdateOrder(int orderId, WriteOrderDto updatedOrder)
        {
            var orderToUpdate = await _dbContext.Orders
                                                .FirstOrDefaultAsync(o => o.OrderId == orderId)
                                                ?? throw new OrderNotFoundException(orderId);

            orderToUpdate.Quantity = updatedOrder.Quantity;
            orderToUpdate.Price = updatedOrder.Price;

            await _dbContext.SaveChangesAsync();

            _matchingEngine.MatchOrders();

            return updatedOrder;
        }

        public async Task<bool> DeleteOrder(int orderId)
        {
            var orderToDelete = await _dbContext.Orders
                                          .FirstOrDefaultAsync(o => o.OrderId == orderId)
                                          ?? throw new OrderNotFoundException(orderId);

                _dbContext.Orders.Remove(orderToDelete);

                return await _dbContext.SaveChangesAsync() > 0 ? true : false;
        }
    }

}

