using RestaurantOrderSystem.Models;
using RestaurantOrderSystem.Domain.Models;

namespace RestaurantOrderSystem.Grains
{
    public interface IOrderGrain : IGrainWithStringKey
    {
        Task CreateOrder(List<OrderItem> items);
        Task<Order> GetOrder();
        Task UpdateOrderStatus(OrderStatus status);
    }
}
