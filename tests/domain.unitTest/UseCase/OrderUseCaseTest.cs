using Core.Notifications;
using Domain.Entities.Enums;
using Domain.Entities.OrderAggregate;
using Domain.Entities.ProductAggregate;
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
        Product productResponseMock;

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


            productResponseMock = new()
            {
                Description = "Hamburguer",
                Name = "X-Bacon",
                ProductType = ProductType.SideDish,
                Price = 30,
                Id = 1
            };

            orderResponseMock = new Order(1, OrderStatus.Creating, [], null, DateTime.Now);
            orderResponseMock.AddProduct(productResponseMock, 1);
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
            Assert.NotEqual(0, result.Id);
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