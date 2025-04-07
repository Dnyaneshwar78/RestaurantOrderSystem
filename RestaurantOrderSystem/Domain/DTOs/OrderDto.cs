namespace RestaurantOrderSystem.Domain.DTOs
{
    public class OrderDto
    {
        public string OrderId { get; set; }
        public List<OrderItemDto> Items { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
