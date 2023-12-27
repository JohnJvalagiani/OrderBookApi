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
            var buyOrdersTask = await _dbContext.Orders
                                          .Where(o => o.OrderType == OrderType.Buy)
                                          .ToListAsync();

            var sellOrdersTask = await _dbContext.Orders
                                           .Where(o => o.OrderType == OrderType.Sell)
                                           .ToListAsync();

            
            var orderBook = new OrderBookModel
            {
                BuyOrders = _mapper.Map<IEnumerable<ReadOrderDto>>(buyOrdersTask),
                SellOrders = _mapper.Map<IEnumerable<ReadOrderDto>>(sellOrdersTask)
            };

            return orderBook;
        }

        public async Task<ReadOrderDto> PlaceBuyOrder(WriteOrderDto order)
        {
            order.OrderType = OrderType.Buy;

            var theOrder =  _dbContext.Orders.Add(_mapper.Map<Order>(order));

            await _dbContext.SaveChangesAsync();

            _matchingEngine.MatchOrders();
            
            return _mapper.Map<ReadOrderDto>(theOrder);
        }

        public async Task<ReadOrderDto> PlaceSellOrder(WriteOrderDto order)
        {
            order.OrderType = OrderType.Sell;

            var theOrder =  _dbContext.Orders.Add(_mapper.Map<Order>(order));

            await _dbContext.SaveChangesAsync();
            
            _matchingEngine.MatchOrders();

            return _mapper.Map<ReadOrderDto>(theOrder);
        }

        public async Task<WriteOrderDto> UpdateOrder(int orderId, WriteOrderDto updatedOrder)
        {
            try
            {
                var orderToUpdate =  _dbContext.Orders
                                                .FirstOrDefault(o => o.OrderId == orderId)
                                                ?? throw new OrderNotFoundException(orderId);

            orderToUpdate.Quantity = updatedOrder.Quantity;
            orderToUpdate.Price = updatedOrder.Price;
          
                var result = _dbContext.Orders.Update(orderToUpdate);
            

            await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new DbUpdateException("Error updating order in the database.", ex);
            }
            catch (Exception ex)
            {
                throw;
            }

            _matchingEngine.MatchOrders();

            return updatedOrder;
        }

        public async Task<bool> DeleteOrder(int orderId)
        {
            var orderToDelete =  _dbContext.Orders
                                          .FirstOrDefault(o => o.OrderId == orderId)
                                          ?? throw new OrderNotFoundException(orderId);

                _dbContext.Orders.Remove(orderToDelete);

                return await _dbContext.SaveChangesAsync() > 0 ? true : false;
        }
    }

}

