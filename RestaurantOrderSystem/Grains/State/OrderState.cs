using RestaurantOrderSystem.Domain.Models;

namespace RestaurantOrderSystem.GrainStates
{
    [GenerateSerializer] // Add Orleans serialization attribute
    public class OrderState
    {
        [Id(0)] public string OrderId { get; set; }
        [Id(1)] public List<OrderItem> Items { get; set; } = new();
        [Id(2)] public decimal TotalAmount { get; set; }
        [Id(3)] public OrderStatus Status { get; set; } = OrderStatus.InProgress;
        [Id(4)] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
