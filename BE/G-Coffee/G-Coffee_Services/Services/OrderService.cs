using AutoMapper;
using G_Cofee_Repositories.DTO;
using G_Cofee_Repositories.IRepositories;
using G_Cofee_Repositories.Models;
using G_Coffee_Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace G_Coffee_Services.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IOrderRepository orderRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<OrderDTO> CreateOrderAsync(OrderDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var entity = _mapper.Map<Order>(dto);
            await _orderRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<OrderDTO>(entity);
        }

        public async Task DeleteOrderAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Order ID is required");

            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {id} not found");

            _orderRepository.Remove(order);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Order ID is required");

            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {id} not found");

            return order;
        }

        public async Task<Order> GetOrderByOrderCodeAsync(long orderCode)
        {
            if (orderCode <= 0)
                throw new ArgumentException("Order code must be greater than zero");

            var order = (await _orderRepository.FindAsync(o => o.OrderCode == orderCode)).FirstOrDefault();
            if (order == null)
                throw new KeyNotFoundException($"Order with code {orderCode} not found");

            return order;
        }

        public async Task<Order> GetOrderByCheckoutUrlAsync(string checkoutUrl)
        {
            if (string.IsNullOrWhiteSpace(checkoutUrl))
                throw new ArgumentException("Checkout URL is required");

            var order = (await _orderRepository.FindAsync(o => o.CheckoutUrl == checkoutUrl)).FirstOrDefault();
            if (order == null)
                throw new KeyNotFoundException($"Order with checkout URL {checkoutUrl} not found");

            return order;
        }

        public async Task UpdateOrderAsync(Order dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var existing = await _orderRepository.GetByIdAsync(dto.Id);
            if (existing == null)
                throw new KeyNotFoundException($"Order with ID {dto.Id} not found");

            _orderRepository.Update(dto);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
