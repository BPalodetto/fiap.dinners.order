using Domain.Entities.Enums;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Interfaces;
using Infrastructure.SqlModels.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Repository.OrderProductTest
{
    public class OrderProductRepositoryTest
    {
        OrderProductRepository _orderProductRepository;

        Mock<DinersOrderSqlContext> _context;
        Mock<IOrderProductRepository> _orderProductSqlRepository;

        public OrderProductRepositoryTest()
        {
            _context = new Mock<DinersOrderSqlContext>();
            _orderProductSqlRepository = new Mock<IOrderProductRepository>();

            _orderProductRepository = new OrderProductRepository(_context.Object);
        }

        [Fact]
        public void DevePermitirAtualizarPedido()
        {
            OrderProductSqlModel orderProductSqlModel = new OrderProductSqlModel()
            {
                Id = 1,
                OrderId = 1,
                ProductId = 1,
                Order = new OrderSqlModel
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

                },
                Quantity = 2,
                UpdatedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
            };

            _orderProductSqlRepository.Setup(x => x.Update(It.IsAny<OrderProductSqlModel>()));
            _context.Setup(x => x.Update(It.IsAny<OrderProductSqlModel>()));

            _orderProductRepository.Update(orderProductSqlModel);

            _orderProductSqlRepository.Verify(p => p.Update(It.IsAny<OrderProductSqlModel>()), Times.Exactly(1));
            _context.Verify(p => p.Update(It.IsAny<OrderProductSqlModel>()), Times.Exactly(1));
        }

        [Fact]
        public void DevePermitirAdicionarPedido()
        {
            OrderProductSqlModel orderProductSqlModel = new OrderProductSqlModel()
            {
                Id = 1,
                OrderId = 1,
                ProductId = 1,
                Order = new OrderSqlModel
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

                },
                ProductPrice = 25,
                Quantity = 2,
                UpdatedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
            };

            _orderProductSqlRepository.Setup(x => x.Add(It.IsAny<OrderProductSqlModel>())).Returns(orderProductSqlModel);
            _context.Setup(x => x.Add(It.IsAny<OrderProductSqlModel>()).Entity).Returns(orderProductSqlModel);

           var result = _orderProductRepository.Add(orderProductSqlModel);

            Assert.NotNull(result);
            Assert.True(orderProductSqlModel.Id == result.Id);
            Assert.True(orderProductSqlModel == result);
            _orderProductSqlRepository.Verify(p => p.Add(It.IsAny<OrderProductSqlModel>()), Times.Exactly(1));
            _context.Verify(p => p.Add(It.IsAny<OrderProductSqlModel>()), Times.Exactly(1));
        }

        [Fact]
        public void DevePermitirRemoverPedido()
        {
            List<OrderProductSqlModel> lstOrderProductSqlModel = new List<OrderProductSqlModel>()
            {
                new OrderProductSqlModel()
                {
                    Id = 1,
                    OrderId = 1,
                    ProductId = 1,
                    Order = new OrderSqlModel
                    {
                        Id = 1,
                        Customer = new Infrastructure.SqlModels.CustomerAggregate.CustomerSqlModel { Name = "Alex" },
                        OrderProducts = new List<OrderProductSqlModel> { new OrderProductSqlModel { } },
                        PaymentKind = PaymentMethodKind.Pix,
                        PaymentProvider = PaymentProvider.MercadoPago,
                        Price = 25,
                        Status = Domain.Entities.Enums.OrderStatus.SentToProduction,
                        CustomerId = 1,
                        UpdatedAt = DateTime.Now,
                        CreatedAt = DateTime.Now,

                    },
                    Quantity = 2,
                    UpdatedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                }
            };

            _orderProductSqlRepository.Setup(x => x.Remove(It.IsAny<List<OrderProductSqlModel>>()));
            _context.Setup(x => x.Update(It.IsAny<OrderProductSqlModel>()));

            _orderProductRepository.Remove(lstOrderProductSqlModel);

            _orderProductSqlRepository.Verify(p => p.Remove(It.IsAny<List<OrderProductSqlModel>>()), Times.Exactly(1));
            _context.Verify(p => p.Remove(It.IsAny<List<OrderProductSqlModel>>()), Times.Exactly(1));
        }
    }
}