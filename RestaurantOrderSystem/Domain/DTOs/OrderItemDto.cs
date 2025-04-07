using System.ComponentModel.DataAnnotations;

namespace RestaurantOrderSystem.Domain.DTOs
{
    public class OrderItemDto
    {
        [Required]
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
