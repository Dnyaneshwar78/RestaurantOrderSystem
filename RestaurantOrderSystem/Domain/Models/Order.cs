using RestaurantOrderSystem.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace RestaurantOrderSystem.Models
{
    [GenerateSerializer]
    public class Order
    {
        [Key]
        [Id(0)]
        public string OrderId { get; set; } // Use as primary key

        [Id(1)]
        public List<OrderItem> Items { get; set; } = new();

        [Id(2)] public decimal TotalAmount { get; set; }
        [Id(3)] public OrderStatus Status { get; set; }
        [Id(4)] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
