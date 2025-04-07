using RestaurantOrderSystem.Domain.Models;
using RestaurantOrderSystem.GrainStates;
using RestaurantOrderSystem.Models;

namespace RestaurantOrderSystem.Grains
{
    public class OrderGrain : Grain, IOrderGrain
    {
        private readonly IPersistentState<OrderState> _state;

        public OrderGrain(
            [PersistentState("order", "OrderStorage")]
            IPersistentState<OrderState> state)
        {
            _state = state;
        }

        public async Task CreateOrder(List<OrderItem> items)
        {
            _state.State.OrderId = this.GetPrimaryKeyString();
            _state.State.Items = items;
            _state.State.TotalAmount = items.Sum(item => item.Price * item.Quantity);
            _state.State.Status = OrderStatus.InProgress;
            _state.State.CreatedAt = DateTime.UtcNow;

            await _state.WriteStateAsync();
        }

        public Task<Order> GetOrder()
        {
            var state = _state.State;

            var order = new Order
            {
                OrderId = state.OrderId,
                Items = state.Items,
                TotalAmount = state.TotalAmount,
                Status = state.Status,
                CreatedAt = state.CreatedAt
            };

            return Task.FromResult(order);
        }

        public async Task UpdateOrderStatus(OrderStatus newStatus)
        {
            _state.State.Status = newStatus;
            await _state.WriteStateAsync();
        }
    }
}
