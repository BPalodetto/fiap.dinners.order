using Domain.Entities.Enums;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Interfaces;
using Infrastructure.SqlModels.OrderAggregate;
using Moq;
using System.Linq.Expressions;

namespace Repository.OrderSqlTest
{
    public class OrderSqlRepositoryTest
    {
        OrderSqlRepository _orderRepository;

        Mock<DinersOrderSqlContext> _context;
        Mock<IOrderSqlRepository> _orderProductSqlRepository;

        public OrderSqlRepositoryTest()
        {
            _context = new Mock<DinersOrderSqlContext>();
            _orderProductSqlRepository = new Mock<IOrderSqlRepository>();

            _orderRepository = new OrderSqlRepository(_context.Object);
        }

        [Fact]
        public async void DevePermitirListarPedido()
        {
            List<OrderSqlModel> orderSqlModels = new List<OrderSqlModel>()
            {
                new OrderSqlModel
                {
                    Id = 1,
                    Customer = new Infrastructure.SqlModels.CustomerAggregate.CustomerSqlModel { Name = "Alex" },
                    OrderProducts = new List<OrderProductSqlModel> { new OrderProductSqlModel { } },
                    PaymentKind = PaymentMethodKind.Pix,
                    PaymentProvider = PaymentProvider.MercadoPago,
                    Price = 25,
                    Status = Domain.Entities.Enums.OrderStatus.Creating,
                    CustomerId = 1,
                    UpdatedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                }
            };
            _orderProductSqlRepository.Setup(x => x.ListAsync(It.IsAny<Expression<Func<OrderSqlModel, bool>>>(),
                                                              It.IsAny<int>(),
                                                              It.IsAny<int>(),
                                                              default)).ReturnsAsync(orderSqlModels);

            var result = await _orderRepository.ListAsync(x => x.Status == OrderStatus.SentToProduction, 1, 10, default);

            Assert.True(result.Any());
            Assert.NotNull(result.First());
        }

        [Fact]
        public async void DevePermitirObterPedido()
        {
            OrderSqlModel orderSqlModels = new OrderSqlModel
            {
                Id = 1,
                Customer = new Infrastructure.SqlModels.CustomerAggregate.CustomerSqlModel { Name = "Alex" },
                OrderProducts = new List<OrderProductSqlModel> { new OrderProductSqlModel { } },
                PaymentKind = PaymentMethodKind.Pix,
                PaymentProvider = PaymentProvider.MercadoPago,
                Price = 25,
                Status = Domain.Entities.Enums.OrderStatus.Creating,
                CustomerId = 1,
                UpdatedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
            };

            _orderProductSqlRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<OrderSqlModel, bool>>>(),
                                                              It.IsAny<bool>(),
                                                              default)).ReturnsAsync(orderSqlModels);


            var result = await _orderRepository.GetAsync(x => x.Id == 1, true, default);

            Assert.NotNull(result);
            Assert.True(result.Id == orderSqlModels.Id);
            Assert.True(result.Customer == orderSqlModels.Customer);
            Assert.True(result.CustomerId == orderSqlModels.CustomerId);
            Assert.True(result.Price == orderSqlModels.Price);
            _orderProductSqlRepository.Verify(x => x.GetAsync(It.IsAny<Expression<Func<OrderSqlModel, bool>>>(),
                                                              It.IsAny<bool>(),
                                                              default), Times.Exactly(1));
        }

        [Fact]
        public void DevePermitirAtualizarPedido()
        {
            OrderSqlModel orderSqlModels = new OrderSqlModel
            {
                Id = 1,
                Customer = new Infrastructure.SqlModels.CustomerAggregate.CustomerSqlModel { Name = "Alex" },
                OrderProducts = new List<OrderProductSqlModel> { new OrderProductSqlModel { } },
                PaymentKind = PaymentMethodKind.Pix,
                PaymentProvider = PaymentProvider.MercadoPago,
                Price = 25,
                Status = Domain.Entities.Enums.OrderStatus.Creating,
                CustomerId = 1,
                UpdatedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
            };


            _orderProductSqlRepository.Setup(x => x.Update(It.IsAny<OrderSqlModel>()));

            _orderRepository.Update(orderSqlModels);

            _orderProductSqlRepository.Verify(x => x.Update(It.IsAny<OrderSqlModel>()), Times.Exactly(1));
        }

        [Fact]
        public void DevePermitirAdicionarPedido()
        {
            OrderSqlModel orderSqlModels = new OrderSqlModel
            {
                Id = 1,
                Customer = new Infrastructure.SqlModels.CustomerAggregate.CustomerSqlModel { Name = "Alex" },
                OrderProducts = new List<OrderProductSqlModel> { new OrderProductSqlModel { } },
                PaymentKind = PaymentMethodKind.Pix,
                PaymentProvider = PaymentProvider.MercadoPago,
                Price = 25,
                Status = Domain.Entities.Enums.OrderStatus.Creating,
                CustomerId = 1,
                UpdatedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
            };

            _orderProductSqlRepository.Setup(x => x.Add(It.IsAny<OrderSqlModel>())).Returns(orderSqlModels);

            var result = _orderRepository.Add(orderSqlModels);

            Assert.NotNull(result);
            Assert.True(result.Id == orderSqlModels.Id);
            Assert.True(result.Customer == orderSqlModels.Customer);
            Assert.True(result.CustomerId == orderSqlModels.CustomerId);
            Assert.True(result.Price == orderSqlModels.Price);
            _orderProductSqlRepository.Verify(x => x.Add(It.IsAny<OrderSqlModel>()), Times.Exactly(1));
        }
    }
}