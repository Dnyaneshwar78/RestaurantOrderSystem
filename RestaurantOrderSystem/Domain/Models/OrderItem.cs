using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantOrderSystem.Domain.Models
{
    [GenerateSerializer] // Orleans serialization
    public class OrderItem
    {
        [Key] // EF Core primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-increment
        public int Id { get; set; }

        [Id(0)] public string Name { get; set; }
        [Id(1)] public int Quantity { get; set; }
        [Id(2)] public decimal Price { get; set; }

        // Optional foreign key if you're using Order navigation
        public string OrderId { get; set; } // Matches Order.OrderId
    }
}
