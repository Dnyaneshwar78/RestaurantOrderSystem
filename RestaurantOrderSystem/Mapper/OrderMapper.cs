using RestaurantOrderSystem.Domain.DTOs;
using RestaurantOrderSystem.Domain.Models;
using RestaurantOrderSystem.Models;

namespace RestaurantOrderSystem.Mapper
{
    public static class OrderMapper
    {
        public static OrderDto ToDto(Order order)
        {
            return new OrderDto
            {
                OrderId = order.OrderId,
                CreatedAt = order.CreatedAt,
                TotalAmount = order.TotalAmount,
                Status = order.Status.ToString(),
                Items = order.Items.Select(ToDto).ToList()
            };
        }

        public static OrderItemDto ToDto(OrderItem item)
        {
            return new OrderItemDto
            {
                Name = item.Name,
                Quantity = item.Quantity,
                Price = item.Price
            };
        }

        public static List<OrderItem> ToOrderItems(List<OrderItemDto> itemDtos)
        {
            return itemDtos.Select(dto => new OrderItem
            {
                Name = dto.Name,
                Quantity = dto.Quantity,
                Price = dto.Price
            }).ToList();
        }
    }
}
