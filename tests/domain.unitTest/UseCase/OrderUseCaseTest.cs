using Core.Notifications;
using Domain.Clients;
using Domain.Entities.CustomerAggregate;
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
        Mock<IPaymentClient> _paymentClient;

        Order orderResponseMock;
        Product productResponseMock;

        public OrderUseCaseTest()
        {

            _orderRepository = new Mock<IOrderRepository>();
            _customerRepository = new Mock<ICustomerRepository>();
            _productRepository = new Mock<IProductRepository>();
            _notificationContext = new Mock<NotificationContext>();
            _paymentClient = new Mock<IPaymentClient>();

            _orderUseCase = new OrderUseCase(_orderRepository.Object, 
                                            _customerRepository.Object, 
                                            _productRepository.Object, 
                                            _notificationContext.Object,
                                            _paymentClient.Object
                                            );


            productResponseMock = new()
            {
                Description = "Hamburguer",
                Name = "X-Bacon",
                ProductType = ProductType.SideDish,
                Price = 25,
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


            Customer customer = new Customer()
            {
                Name = "Alex",
                Email = "alex@email.com",
                Cpf = "333.824.233-67",
                Id = 938
            };


            Product product = new() { Description = "Hambuguer", Name = "x-salada", Price = 25, ProductType = ProductType.SideDish };

            _productRepository.Setup(x => x.GetAsync(It.IsAny<int>(), default)).ReturnsAsync(product);
            _customerRepository.Setup(x => x.GetAsync(It.IsAny<int>(), default)).ReturnsAsync(customer);
            _orderRepository.Setup(x => x.CreateAsync(It.IsAny<Order>(), default)).ReturnsAsync(orderResponseMock);

            var result = await _orderUseCase.CreateAsync(createOrderRequest, default);

            Assert.NotNull(result);
            Assert.NotEqual(0, result.Id);
        }

        [Fact]
        public async void DevePermitirObterPedido()
        {
            _orderRepository.Setup(x => x.GetAsync(It.IsAny<int>(), default)).ReturnsAsync(orderResponseMock);

            var result = await _orderUseCase.GetAsync(1, default);
            
            Assert.NotNull(result);
            Assert.True(result.Price > 0);
        }

        [Fact]
        public async void DevePermitirAdicionarPedido()
        {
            OrderAddProductRequest orderAddProductRequest = new OrderAddProductRequest()
            {
                ProductId = 1,
                Quantity = 1,
            };

            Product product = new() { Description = "Hambuguer", Name = "x-salada", Price = 25, ProductType = ProductType.SideDish };

            _orderRepository.Setup(x => x.GetAsync(It.IsAny<int>(), default)).ReturnsAsync(orderResponseMock);
            _productRepository.Setup(x => x.GetAsync(It.IsAny<int>(), default)).ReturnsAsync(product);

            var result = await _orderUseCase.AddProduct(1, orderAddProductRequest, default);

            Assert.NotNull(result);
            Assert.True(result.Price > 0);
        }

        [Fact]
        public async void DevePermitirRemoverPedido()
        {
            _orderRepository.Setup(x => x.GetAsync(It.IsAny<int>(), default)).ReturnsAsync(orderResponseMock);
            _orderRepository.Setup(x => x.UpdateAsync(It.IsAny<Order>(), default));

            var result = await _orderUseCase.RemoveProduct(1, 1, default);

            Assert.NotNull(result);
            Assert.True(result.Id == 1);
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