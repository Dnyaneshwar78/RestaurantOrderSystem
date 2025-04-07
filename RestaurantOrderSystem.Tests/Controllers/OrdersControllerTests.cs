using Microsoft.AspNetCore.Mvc;
using Moq;
using RestaurantOrderSystem.Controllers;
using RestaurantOrderSystem.Domain.DTOs;
using RestaurantOrderSystem.Domain.Models;
using RestaurantOrderSystem.Grains;
using RestaurantOrderSystem.Models;


namespace RestaurantOrderSystem.Tests.Controllers
{
   public class OrdersControllerTests
    {
        [Fact]
        public async Task CreateOrder_ReturnsOk_WhenValidRequest()
        {
            // Arrange
            var mockFactory = new Mock<IGrainFactory>();
            var mockOrderGrain = new Mock<IOrderGrain>();

            var orderId = Guid.NewGuid().ToString();
            var items = new List<OrderItem>
            {
                new OrderItem { Name = "Burger", Quantity = 2, Price = 5.99m }
            };

            var order = new Order
            {
                OrderId = orderId,
                Items = items,
                TotalAmount = items.Sum(i => i.Price * i.Quantity),
                CreatedAt = DateTime.UtcNow,
                Status = OrderStatus.InProgress
            };

            mockOrderGrain.Setup(x => x.CreateOrder(It.IsAny<List<OrderItem>>())).Returns(Task.CompletedTask);
            mockOrderGrain.Setup(x => x.GetOrder()).ReturnsAsync(order);

            mockFactory.Setup(f => f.GetGrain<IOrderGrain>(It.IsAny<string>(), null)).Returns(mockOrderGrain.Object);

            var controller = new OrdersController(mockFactory.Object);

            var request = new OrderRequestDto
            {
                Items = new List<OrderItemDto> {
                    new OrderItemDto { Name = "Burger", Quantity = 2, Price = 5.99m }
                }
            };

            // Act
            var result = await controller.CreateOrder(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedOrder = Assert.IsType<OrderDto>(okResult.Value);
            Assert.Equal(order.TotalAmount, returnedOrder.TotalAmount);
            Assert.Equal(order.Items.Count, returnedOrder.Items.Count);
        }


        [Fact]
        public async Task GetOrder_ReturnsOk_WhenOrderExists()
        {
            // Arrange
            var mockFactory = new Mock<IGrainFactory>();
            var mockOrderGrain = new Mock<IOrderGrain>();
            var orderId = Guid.NewGuid().ToString();

            var order = new Order
            {
                OrderId = orderId,
                Items = new List<OrderItem> { new OrderItem { Name = "Pizza", Quantity = 1, Price = 12.99m } },
                TotalAmount = 12.99m,
                Status = OrderStatus.InProgress,
                CreatedAt = DateTime.UtcNow
            };

            mockOrderGrain.Setup(g => g.GetOrder()).ReturnsAsync(order);
            mockFactory.Setup(f => f.GetGrain<IOrderGrain>(orderId, null)).Returns(mockOrderGrain.Object);

            var controller = new OrdersController(mockFactory.Object);

            // Act
            var result = await controller.GetOrder(orderId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<OrderDto>(okResult.Value);
            Assert.Equal(order.OrderId, dto.OrderId);
        }

        [Fact]
        public async Task GetOrder_ReturnsNotFound_WhenOrderIsNull()
        {
            // Arrange
            var mockFactory = new Mock<IGrainFactory>();
            var mockOrderGrain = new Mock<IOrderGrain>();
            var orderId = Guid.NewGuid().ToString();

            mockOrderGrain.Setup(g => g.GetOrder()).ReturnsAsync((Order?)null);
            mockFactory.Setup(f => f.GetGrain<IOrderGrain>(orderId, null)).Returns(mockOrderGrain.Object);

            var controller = new OrdersController(mockFactory.Object);

            // Act
            var result = await controller.GetOrder(orderId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }



        [Fact]
        public async Task UpdateOrderStatus_ReturnsNoContent_WhenStatusUpdated()
        {
            // Arrange
            var mockFactory = new Mock<IGrainFactory>();
            var mockOrderGrain = new Mock<IOrderGrain>();
            var orderId = Guid.NewGuid().ToString();
            var statusUpdate = new OrderStatusUpdateRequest { Status = OrderStatus.Completed };

            mockOrderGrain.Setup(g => g.UpdateOrderStatus(statusUpdate.Status)).Returns(Task.CompletedTask);
            mockFactory.Setup(f => f.GetGrain<IOrderGrain>(orderId, null)).Returns(mockOrderGrain.Object);

            var controller = new OrdersController(mockFactory.Object);

            // Act
            var result = await controller.UpdateOrderStatus(orderId, statusUpdate);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }



    }
}
