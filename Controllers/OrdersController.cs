using Microsoft.AspNetCore.Mvc;
using RestaurantManagementSystem.DTOs.Requests;
using RestaurantManagementSystem.Services;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateOrder([FromBody] OrderDTO orderDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    try
        //    {
        //        var newOrder = await _orderService.CreateOrderAsync(orderDto);
        //        return CreatedAtAction(nameof(GetOrder), new { id = newOrder.Id }, newOrder);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpPut("{id}/status")]
        //public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] OrderUpdateStatusDTO statusDto)
        //{
        //    var result = await _orderService.UpdateOrderStatusAsync(id, statusDto);
        //    if (!result)
        //    {
        //        return NotFound();
        //    }
        //    return NoContent();
        //}
    }
}