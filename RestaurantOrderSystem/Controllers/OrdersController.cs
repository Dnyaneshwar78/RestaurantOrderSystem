using Microsoft.AspNetCore.Mvc;
using RestaurantOrderSystem.Domain.DTOs;
using RestaurantOrderSystem.Domain.Models;
using RestaurantOrderSystem.Grains;
using RestaurantOrderSystem.Mapper;

namespace RestaurantOrderSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IGrainFactory _grainFactory;

        public OrdersController(IGrainFactory grainFactory)
        {
            _grainFactory = grainFactory;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequestDto requestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var orderId = Guid.NewGuid().ToString();
            var orderGrain = _grainFactory.GetGrain<IOrderGrain>(orderId);

            var orderItems = OrderMapper.ToOrderItems(requestDto.Items);

            await orderGrain.CreateOrder(orderItems);
            var createdOrder = await orderGrain.GetOrder();

            var orderDto = OrderMapper.ToDto(createdOrder);
            return Ok(orderDto);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder(string orderId)
        {
            var orderGrain = _grainFactory.GetGrain<IOrderGrain>(orderId);
            var order = await orderGrain.GetOrder();
            return order is null ? NotFound() : Ok(OrderMapper.ToDto(order));
        }

        [HttpPut("{orderId}/status")]
        public async Task<IActionResult> UpdateOrderStatus(string orderId, [FromBody] OrderStatusUpdateRequest request)
        {
            var orderGrain = _grainFactory.GetGrain<IOrderGrain>(orderId);
            await orderGrain.UpdateOrderStatus(request.Status);
            return NoContent();
        }
    }
}
