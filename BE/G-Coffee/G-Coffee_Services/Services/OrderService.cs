using AutoMapper;
using G_Cofee_Repositories.DTO;
using G_Cofee_Repositories.IRepositories;
using G_Cofee_Repositories.Models;
using G_Coffee_Services.IServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Coffee_Services.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _OrderRepository;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IOrderRepository OrderRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _OrderRepository = OrderRepository ?? throw new ArgumentNullException(nameof(OrderRepository));
            _mapper = mapper;
        }

        public async Task<OrderDTO> CreateOrderAsync(OrderDTO Order)
        {
            if (Order == null) throw new ArgumentNullException(nameof(Order));

            try
            {
                var OrderEntity = _mapper.Map<Order>(Order);

                await _OrderRepository.AddAsync(OrderEntity);
                await _unitOfWork.SaveChangesAsync();

                // Fix: Map the saved OrderEntity to OrderDTO before returning
                var OrderDto = _mapper.Map<OrderDTO>(OrderEntity);
                return OrderDto;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating Order", ex);
            }
        }

        public async Task DeleteOrderAsync(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentException("Order ID is required");

            try
            {
                var Order = await _OrderRepository.GetByIdAsync(id);
                if (Order == null) throw new Exception($"Order with ID {id} not found");

                _OrderRepository.Remove(Order);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting Order with ID {id}", ex);
            }
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            try
            {
                var Orders = await _OrderRepository.GetAllAsync();
                return Orders;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving all Orders", ex);
            }
        }

        public async Task<Order> GetOrderByIdAsync(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentException("Order ID is required");

            try
            {
                var Order = await _OrderRepository.GetByIdAsync(id);
                if (Order == null) throw new Exception($"Order with ID {id} not found");

                return Order;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving Order with ID {id}", ex);
            }
        }
        public async Task<Order> GetOrderByOrderCodeAsync(long orderCode)
        {
            if (orderCode <= 0) throw new ArgumentException("Order ID must be greater than zero");

            try
            {
                // Fix: Await the result of FindAsync and then use LINQ's FirstOrDefault to retrieve a single Order
                var orders = await _OrderRepository.FindAsync(o => o.OrderCode == orderCode);
                var order = orders.FirstOrDefault();
                if (order == null) throw new Exception($"Order with Code {orderCode} not found");

                return order;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving Order with Code {orderCode}", ex);
            }
        }

        public async Task UpdateOrderAsync(Order Order)
        {
            if (Order == null) throw new ArgumentNullException(nameof(Order));

            try
            {
                var existingOrder = await _OrderRepository.GetByIdAsync(Order.Id);
                if (existingOrder == null) throw new Exception($"Order with ID {Order.Id} not found");

                _OrderRepository.Update(Order);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating Order with ID {Order.Id}", ex);
            }
        }
        public async Task<Order> GetOrderByCheckoutUrlAsync(string checkoutUrl)
        {
            if (string.IsNullOrWhiteSpace(checkoutUrl)) throw new ArgumentException("Checkout URL is required");

            try
            {
                // Fix: Use LINQ's FirstOrDefault to retrieve a single Order from the IEnumerable result
                var order = (await _OrderRepository.FindAsync(o => o.CheckoutUrl == checkoutUrl)).FirstOrDefault();
                if (order == null) throw new Exception($"Order with Checkout URL {checkoutUrl} not found");

                return order;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving Order with Checkout URL {checkoutUrl}", ex);
            }
        }
    }
}
