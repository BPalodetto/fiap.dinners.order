using Core.Notifications;
using Domain.Entities.Enums;
using Domain.Entities.OrderAggregate;
using Domain.Repositories;
using Moq;
using UseCase.Dtos.OrderRequest;
using UseCase.Services;

namespace UseCase.OrderTest
{
    public class OrderUseCaseTest
    {
        OrderUseCase _orderUseCase;

        Mock<IOrderRepository> _orderRepository;
        Mock<ICustomerRepository> _customerRepository;
        Mock<IProductRepository> _productRepository;
        Mock<NotificationContext> _notificationContext;

        Order orderResponseMock;
        

        public OrderUseCaseTest()
        {

            _orderRepository = new Mock<IOrderRepository>();
            _customerRepository = new Mock<ICustomerRepository>();
            _productRepository = new Mock<IProductRepository>();
            _notificationContext = new Mock<NotificationContext>();

            _orderUseCase = new OrderUseCase(_orderRepository.Object, 
                                            _customerRepository.Object, 
                                            _productRepository.Object, 
                                            _notificationContext.Object
                                            );

            List<OrderProduct> lstOrderProducts = new List<OrderProduct>();
            OrderProduct order = null;

            order.Quantity = 1;
            lstOrderProducts.Add(order);

            orderResponseMock = new Order(1, OrderStatus.Creating, lstOrderProducts, null, DateTime.Now);
        }

        [Fact]
        public async void DevePermitirCriarPedido()
        {
            CreateOrderRequest createOrderRequest = new CreateOrderRequest()
            {
                CustomerId = 1,
                Product = new OrderAddProductRequest
                {
                    ProductId = 1,
                    Quantity = 1
                }
            };

            _orderRepository.Setup(x => x.CreateAsync(It.IsAny<Order>(), default)).Returns(() =>
            {
                Task.FromResult(orderResponseMock);
            });

            var result = await _orderUseCase.CreateAsync(createOrderRequest, default);

            Assert.NotNull(result);
        }

        [Fact]
        public async void DevePermitirObterPedido()
        {

            _orderRepository.Setup(x => x.GetAsync(It.IsAny<int>(), default)).Returns(() =>
            {
                Task.FromResult(orderResponseMock);
            });

            var result = await _orderUseCase.GetAsync(1, default);
            
            Assert.NotNull(result);
            Assert.True(result.Price > 0);
            Assert.True(result.CustomerId > 0);
        }

        [Fact]
        public async void DevePermitirAdicionarPedido()
        {
            OrderAddProductRequest orderAddProductRequest = new OrderAddProductRequest()
            {
                ProductId = 1,
                Quantity = 1,
            };

            var result = await _orderUseCase.AddProduct(1, orderAddProductRequest, default);

            Assert.NotNull(result);
            Assert.True(result.Price > 0);
            Assert.True(result.CustomerId > 0);
        }

        [Fact]
        public async void DevePermitirRemoverPedido()
        {
            var result = await _orderUseCase.RemoveProduct(1, 1, default);

            Assert.NotNull(result);
            Assert.True(result.Price > 0);
            Assert.True(result.CustomerId > 0);
        }


        [Fact]
        public async void DevePermitirAtualizarStatusPedido()
        {
            OrderAddProductRequest orderAddProductRequest = new OrderAddProductRequest()
            {
                ProductId = 1,
                Quantity = 1,
            };

            await _orderUseCase.UpdateStatusToSentToProduction(1, default);
        }
    }
}