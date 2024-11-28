using Domain.Entities.Enums;
using Domain.Entities.OrderAggregate;
using Infrastructure.Adapters;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Interfaces;
using Infrastructure.SqlModels.OrderAggregate;
using Moq;
using System.Linq.Expressions;

namespace Adapters.OrderRepositoryTest
{
    public class OrderRepositoryAdpaterTest
    {
        OrderRepositoryAdpater _orderRepositoryAdapter;

        Mock<IOrderSqlRepository> _orderSqlRepository;

        Mock<IOrderProductRepository> _orderProductRepository;
        public OrderRepositoryAdpaterTest()
        {
            _orderSqlRepository = new Mock<IOrderSqlRepository>();
            _orderProductRepository = new Mock<IOrderProductRepository>();

            _orderRepositoryAdapter = new OrderRepositoryAdpater(_orderSqlRepository.Object,
                                                                 _orderProductRepository.Object);
        }

        [Fact]
        public async void DevePermitirCriarPedido()
        {
            Order order = new Order
            {
                Id = 1,
                CustomerId = 1,
                CreatedDate = DateTime.Now,
            };

            OrderSqlModel orderSqlModel = new OrderSqlModel
            {
                Id = 1,
                Customer = new Infrastructure.SqlModels.CustomerAggregate.CustomerSqlModel { Name = "Alex", Id = 1 },
                //OrderProducts = new List<OrderProductSqlModel> 
                //{
                //    new OrderProductSqlModel
                //    { 
                //        Product = new Infrastructure.SqlModels.ProductAggregate.ProductSqlModel 
                //        {
                //            Id=1,
                //            Name = "Hamburguer",
                //            Description = "Hamburguer fiap",
                //            ProductType = ProductType.SideDish,
                //            Price = 25,
                //            OrderProducts = new List<OrderProductSqlModel>
                //            {
                //                new OrderProductSqlModel
                //                {
                //                    ProductId = 1,
                //                    ProductPrice = 25,
                //                }
                //            }
                //        } 
                //    } 
                //},
                PaymentKind = PaymentMethodKind.Pix,
                PaymentProvider = PaymentProvider.MercadoPago,
                Price = 25,
                Status = Domain.Entities.Enums.OrderStatus.Creating,
                CustomerId = 1,
                UpdatedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
            };

            _orderSqlRepository.Setup(x => x.Add(It.IsAny<OrderSqlModel>())).Returns(orderSqlModel);
            _orderSqlRepository.Setup(x => x.UnitOfWork.CommitAsync(default)).ReturnsAsync(1);
            _orderSqlRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<OrderSqlModel, bool>>>(),
                                                      It.IsAny<bool>(),
                                                      default
                                                      )).ReturnsAsync(orderSqlModel);

            var result = await _orderRepositoryAdapter.CreateAsync(order, default);

            Assert.NotNull(result);
            Assert.True(result.Id == orderSqlModel.Id);
            Assert.True(result.CustomerId == orderSqlModel.CustomerId);
            _orderSqlRepository.Verify(x => x.UnitOfWork, Times.Once());
            _orderSqlRepository.Verify(x => x.Add(It.IsAny<OrderSqlModel>()), Times.Once());
        }

        [Fact]
        public async void DevePermitirObterPedido()
        {
            OrderSqlModel orderSqlModel = new OrderSqlModel
            {
                Id = 1,
                Customer = new Infrastructure.SqlModels.CustomerAggregate.CustomerSqlModel { Name = "Alex", Id = 1 },
                //OrderProducts = new List<OrderProductSqlModel> 
                //{
                //    new OrderProductSqlModel
                //    { 
                //        Product = new Infrastructure.SqlModels.ProductAggregate.ProductSqlModel 
                //        {
                //            Id=1,
                //            Name = "Hamburguer",
                //            Description = "Hamburguer fiap",
                //            ProductType = ProductType.SideDish,
                //            Price = 25,
                //            OrderProducts = new List<OrderProductSqlModel>
                //            {
                //                new OrderProductSqlModel
                //                {
                //                    ProductId = 1,
                //                    ProductPrice = 25,
                //                }
                //            }
                //        } 
                //    } 
                //},
                PaymentKind = PaymentMethodKind.Pix,
                PaymentProvider = PaymentProvider.MercadoPago,
                Price = 25,
                Status = Domain.Entities.Enums.OrderStatus.Creating,
                CustomerId = 1,
                UpdatedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
            };

            _orderSqlRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<OrderSqlModel, bool>>>(),
                                                      It.IsAny<bool>(), 
                                                      default
                                                      )).ReturnsAsync(orderSqlModel);

            _orderSqlRepository.Setup(x => x.UnitOfWork.CommitAsync(default)).ReturnsAsync(1);


            var result = await _orderRepositoryAdapter.GetAsync(1, default);

            Assert.NotNull(result);
            Assert.True(result.Id == orderSqlModel.Id);
            Assert.True(result.CustomerId == orderSqlModel.CustomerId);
            Assert.True(result.Status == orderSqlModel.Status);
            _orderSqlRepository.Verify(x => x.GetAsync(It.IsAny<Expression<Func<OrderSqlModel, bool>>>(),
                                                      It.IsAny<bool>(),
                                                      default
                                                      ), Times.Exactly(1));
        }

        [Fact]
        public async void DevePermitirListarPedido()
        {
            List<OrderSqlModel> lstorderSqlModel = new List<OrderSqlModel>();
            OrderSqlModel orderSqlModel = new OrderSqlModel
            {
                Id = 1,
                Customer = new Infrastructure.SqlModels.CustomerAggregate.CustomerSqlModel { Name = "Alex", Id = 1 },
                OrderProducts = new List<OrderProductSqlModel> { new OrderProductSqlModel { } },
                PaymentKind = PaymentMethodKind.Pix,
                PaymentProvider = PaymentProvider.MercadoPago,
                Price = 25,
                Status = Domain.Entities.Enums.OrderStatus.Creating,
                CustomerId = 1,
                UpdatedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
            };

            lstorderSqlModel.Add(orderSqlModel);
            _orderSqlRepository.Setup(x => x.ListAsync(It.IsAny<Expression<Func<OrderSqlModel, bool>>>(),
                                                      It.IsAny<int>(), 
                                                      It.IsAny<int>(),
                                                      default
                                                      )).ReturnsAsync(lstorderSqlModel);
            
            var result = await _orderRepositoryAdapter.ListAsync(OrderStatus.Creating, 1, 10, default);

            Assert.NotNull(result);
            Assert.True(result.Any());
            _orderSqlRepository.Verify(p => p.ListAsync(It.IsAny<Expression<Func<OrderSqlModel, bool>>>(),
                                                         It.IsAny<int>(),
                                                         It.IsAny<int>(),
                                                         default), Times.Exactly(1));
        }

        [Fact]
        public async void DevePermitirListarPedidoPorListaStatus()
        {
            List<OrderStatus> lstOrderStatus = new List<OrderStatus>();
            lstOrderStatus.Add(OrderStatus.Creating);

            List<OrderSqlModel> lstorderSqlModel = new List<OrderSqlModel>();
            OrderSqlModel orderSqlModel = new OrderSqlModel
            {
                Id = 1,
                Customer = new Infrastructure.SqlModels.CustomerAggregate.CustomerSqlModel { Name = "Alex", Id = 1 },
                OrderProducts = new List<OrderProductSqlModel> { new OrderProductSqlModel { } },
                PaymentKind = PaymentMethodKind.Pix,
                PaymentProvider = PaymentProvider.MercadoPago,
                Price = 25,
                Status = Domain.Entities.Enums.OrderStatus.Creating,
                CustomerId = 1,
                UpdatedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
            };

            lstorderSqlModel.Add(orderSqlModel);

            _orderSqlRepository.Setup(x => x.ListAsync(It.IsAny<Expression<Func<OrderSqlModel, bool>>>(),
                                                      It.IsAny<int>(),
                                                      It.IsAny<int>(),
                                                      default
                                                      )).ReturnsAsync(lstorderSqlModel);

            var result = await _orderRepositoryAdapter.ListAsync(lstOrderStatus, 1, 10, default);

             Assert.NotNull(result);
            Assert.True(result.Any());
            _orderSqlRepository.Verify(p => p.ListAsync(It.IsAny<Expression<Func<OrderSqlModel, bool>>>(),
                                                         It.IsAny<int>(),
                                                         It.IsAny<int>(),
                                                         default), Times.Exactly(1));
        }

        [Fact]
        public async void DevePermitirAtualizarPedido()
        {
            Order order = new Order
            {
                Id = 1,
                CustomerId = 1,
                CreatedDate = DateTime.Now,
            };

            OrderSqlModel orderSqlModel = new OrderSqlModel
            {
                Id = 1,
                Customer = new Infrastructure.SqlModels.CustomerAggregate.CustomerSqlModel { Name = "Alex", Id = 1 },
                PaymentKind = PaymentMethodKind.Pix,
                PaymentProvider = PaymentProvider.MercadoPago,
                Price = 25,
                Status = Domain.Entities.Enums.OrderStatus.Creating,
                CustomerId = 1,
                UpdatedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
            };

            _orderSqlRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<OrderSqlModel, bool>>>(),
                                                      It.IsAny<bool>(),
                                                      default
                                                      )).ReturnsAsync(orderSqlModel);

            _orderSqlRepository.Setup(x => x.Update(It.IsAny<OrderSqlModel>()));
            _orderSqlRepository.Setup(x => x.UnitOfWork.CommitAsync(default)).ReturnsAsync(1);

           await _orderRepositoryAdapter.UpdateAsync(order, default);
        }
    }
}