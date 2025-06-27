using G_Cofee_Repositories.DTO;
using G_Cofee_Repositories.Models;
using G_Coffee_Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace G_Coffee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _OrderService;

        public OrderController(IOrderService OrderService)
        {
            _OrderService = OrderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDTO Order)
        {
            if (Order == null) return BadRequest("Order cannot be null");
            var createdOrder = await _OrderService.CreateOrderAsync(Order);
            return CreatedAtAction(nameof(GetOrder), new { id = createdOrder.Id }, createdOrder);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(Guid id)
        {
            var Order = await _OrderService.GetOrderByIdAsync(id);
            if (Order == null) return NotFound();
            return Ok(Order);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var Orders = await _OrderService.GetAllOrdersAsync();
            return Ok(Orders);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(string id, [FromBody] Order Order)
        {
            if (!Guid.TryParse(id, out var parsedId)) return BadRequest("Invalid Order ID format");
            if (parsedId != Order.Id) return BadRequest("Order ID mismatch");
            await _OrderService.UpdateOrderAsync(Order);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            await _OrderService.DeleteOrderAsync(id);
            return NoContent();
        }

        [HttpGet("Order/{OrderId}")]
        public async Task<IActionResult> GetOrdersByOrderId(Guid OrderId)
        {
            var Orders = await _OrderService.GetOrderByIdAsync(OrderId);
            return Ok(Orders);
        }
    }
}
