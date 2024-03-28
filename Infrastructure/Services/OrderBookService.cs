using Application.CustomExceptions;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;

namespace Infrastructure.Services
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

        public async Task<ServiceResponse<OrderBookModel>> GetOrderBook()
        {
            var response = new ServiceResponse<OrderBookModel> ();
            try
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

                response.Data = orderBook;
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = $"Error fetching order book: {ex.Message}";
                return response;
            }
        }


        public async Task<ServiceResponse<ReadOrderDto>> PlaceBuyOrder(WriteOrderDto order)
        {
            var response = new ServiceResponse<ReadOrderDto>();

            try
            {
                order.OrderType = OrderType.Buy;

                var theOrder = _dbContext.Orders.Add(_mapper.Map<Order>(order));

                _dbContext.SaveChanges();

                _matchingEngine.MatchOrders();

                response.Data = _mapper.Map<ReadOrderDto>(theOrder.Entity);
                response.IsSuccess = true;

                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = $"Error placing buy order: {ex.Message}";

                return response;
            }
        }


        public async Task<ServiceResponse<ReadOrderDto>> PlaceSellOrder(WriteOrderDto order)
        {
            var response = new ServiceResponse<ReadOrderDto>();

            try
            {
                order.OrderType = OrderType.Sell;

                var theOrder = _dbContext.Orders.Add(_mapper.Map<Order>(order));

                _dbContext.SaveChanges();

                _matchingEngine.MatchOrders();

                response.Data = _mapper.Map<ReadOrderDto>(theOrder.Entity);
                response.IsSuccess = true;

                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = $"Error placing sell order: {ex.Message}";

                return response;
            }
        }


        public async Task<ServiceResponse<WriteOrderDto>> UpdateOrder(int orderId, WriteOrderDto updatedOrder)
        {
            var response = new ServiceResponse<WriteOrderDto>();

            try
            {
                var orderToUpdate = _dbContext.Orders
                                               .FirstOrDefault(o => o.OrderId == orderId)
                                               ?? throw new OrderNotFoundException(orderId);

                orderToUpdate.Quantity = updatedOrder.Quantity;
                orderToUpdate.Price = updatedOrder.Price;

                var result = _dbContext.Orders.Update(orderToUpdate);

                await _dbContext.SaveChangesAsync();

                _matchingEngine.MatchOrders();

                response.Data = updatedOrder;
                response.IsSuccess = true;

                return response;
            }
            catch (DbUpdateException ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = $"Error updating order in the database: {ex.Message}";

                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = $"An unexpected error occurred: {ex.Message}";

                return response;
            }
        }


        public async Task<ServiceResponse<bool>> DeleteOrder(int orderId)
        {
            var response = new ServiceResponse<bool>();

            try
            {
                var orderToDelete = _dbContext.Orders
                                              .FirstOrDefault(o => o.OrderId == orderId)
                                              ?? throw new OrderNotFoundException(orderId);

                _dbContext.Orders.Remove(orderToDelete);

                response.Data = await _dbContext.SaveChangesAsync() > 0;
                response.IsSuccess = true;

                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = $"Error deleting order: {ex.Message}";

                return response;
            }
        }

    }

}

