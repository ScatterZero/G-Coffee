using G_Cofee_Repositories.DTO;
using G_Cofee_Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Coffee_Services.IServices
{
    public interface IOrderService
    {
        Task<OrderDTO> CreateOrderAsync(OrderDTO Order);
        Task<Order> GetOrderByIdAsync(Guid id);
        Task<Order> GetOrderByOrderCodeAsync(long id);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task UpdateOrderAsync(Order Order);
        Task DeleteOrderAsync(Guid id);
        Task<Order> GetOrderByCheckoutUrlAsync(string checkoutUrl);
    }
}
