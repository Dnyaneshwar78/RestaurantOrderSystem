using System.ComponentModel.DataAnnotations;

namespace RestaurantOrderSystem.Domain.DTOs
{
    public class OrderRequestDto
    {
        [Required]
        [MinLength(1, ErrorMessage = "At least one item is required")]
        public List<OrderItemDto> Items { get; set; }
    }
}
