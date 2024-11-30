using Domain.Entities.Enums;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Interfaces;
using Infrastructure.SqlModels.ProductAggregate;
using Moq;
using System.Linq.Expressions;

namespace Repository.ProductSqlTest
{
    public class ProductSqlRepositoryTest
    {
        ProductSqlRepository _orderRepository;

        Mock<DinersOrderSqlContext> _context;
        Mock<IProductSqlRepository> _productSqlRepository;

        byte[] byteArray = null;

        public ProductSqlRepositoryTest()
        {
            _context = new Mock<DinersOrderSqlContext>();
            _productSqlRepository = new Mock<IProductSqlRepository>();

            _orderRepository = new ProductSqlRepository(_context.Object);
        }

        [Fact]
        public async void DevePermitirListarProduto()
        {
            List<ProductSqlModel> products = new List<ProductSqlModel>()
            {
                new ProductSqlModel
                {
                    Description = "Hamburguer",
                    Name = "X-Bacon",
                    ProductType = ProductType.Drink,
                    Price = 30,
                    UpdatedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    PhotoFilename = "perfil.png",
                    PhotoContentType = "image/jpeg",
                    PhotoData = byteArray,
                    OrderProducts = new List<Infrastructure.SqlModels.OrderAggregate.OrderProductSqlModel>
                    {
                        new Infrastructure.SqlModels.OrderAggregate.OrderProductSqlModel
                        {
                            OrderId = 1,
                            ProductId = 1,
                        }
                    }
                }
            };

            _productSqlRepository.Setup(x => x.ListAsync(It.IsAny<Expression<Func<ProductSqlModel, bool>>>(),
                                                         It.IsAny<int>(),
                                                         It.IsAny<int>(),
                                                         default)).ReturnsAsync(products);


            var result = await _orderRepository.ListAsync(x => x.ProductType == ProductType.Drink, 1, 10, default);

            Assert.NotNull(result);
            Assert.True(result.Any());
            _productSqlRepository.Verify(p => p.ListAsync(It.IsAny<Expression<Func<ProductSqlModel, bool>>>(),
                                                         It.IsAny<int>(),
                                                         It.IsAny<int>(),
                                                         default), Times.Exactly(1));
        }

        [Fact]
        public async void DevePermitirObterProduto()
        {
            ProductSqlModel product = new ProductSqlModel
            {
                Description = "Hamburguer",
                Name = "X-Bacon",
                ProductType = ProductType.Drink,
                Price = 30,
                UpdatedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
                PhotoFilename = "perfil.png",
                PhotoContentType = "image/jpeg",
                PhotoData = byteArray,
                OrderProducts = new List<Infrastructure.SqlModels.OrderAggregate.OrderProductSqlModel>
                    {
                        new Infrastructure.SqlModels.OrderAggregate.OrderProductSqlModel
                        {
                            OrderId = 1,
                            ProductId = 1,
                        }
                    }
            };

            _productSqlRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<ProductSqlModel, bool>>>(),
                                                        It.IsAny<bool>(),
                                                        default)).ReturnsAsync(product);

            var result = await _orderRepository.GetAsync(x => x.Id == 1, true, default);


            Assert.NotNull(result);
            Assert.Contains(result.Name, product.Name);
            Assert.True(result.ProductType == product.ProductType);
            Assert.True(result.Price > 0);
            Assert.True(result.Price == product.Price);
            _productSqlRepository.Verify(p => p.GetAsync(It.IsAny<Expression<Func<ProductSqlModel, bool>>>(),
                                                        It.IsAny<bool>(),
                                                        default), Times.Exactly(1));

        }

        [Fact]
        public void DevePermitirAtualizarProduto()
        {
            ProductSqlModel product = new ProductSqlModel
            {
                Description = "Hamburguer",
                Name = "X-Bacon",
                ProductType = ProductType.Drink,
                Price = 30,
                UpdatedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
                PhotoFilename = "perfil.png",
                PhotoContentType = "image/jpeg",
                PhotoData = byteArray,
                OrderProducts = new List<Infrastructure.SqlModels.OrderAggregate.OrderProductSqlModel>
                    {
                        new Infrastructure.SqlModels.OrderAggregate.OrderProductSqlModel
                        {
                            OrderId = 1,
                            ProductId = 1,
                        }
                    }
            };

            _productSqlRepository.Setup(x => x.Update(It.IsAny<ProductSqlModel>()));

            _orderRepository.Update(product);

            _productSqlRepository.Verify(p => p.Update(It.IsAny<ProductSqlModel>()), Times.Exactly(1));
        }

        [Fact]
        public void DevePermitirAdicionarProduto()
        {
            ProductSqlModel product = new ProductSqlModel
            {
                Description = "Hamburguer",
                Name = "X-Bacon",
                ProductType = ProductType.Drink,
                Price = 30,
                UpdatedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
                PhotoFilename = "perfil.png",
                PhotoContentType = "image/jpeg",
                PhotoData = byteArray,
                OrderProducts = new List<Infrastructure.SqlModels.OrderAggregate.OrderProductSqlModel>
                    {
                        new Infrastructure.SqlModels.OrderAggregate.OrderProductSqlModel
                        {
                            OrderId = 1,
                            ProductId = 1,
                        }
                    }
            };

            _productSqlRepository.Setup(x => x.Add(It.IsAny<ProductSqlModel>())).Returns(product);

            var result = _orderRepository.Add(product);

            Assert.NotNull(result);
            Assert.Contains(result.Name, product.Name);
            Assert.Contains(result.Description, product.Description);
            Assert.True(result.Price > 0);
            Assert.True(result.Price == product.Price);
            _productSqlRepository.Verify(p => p.Add(It.IsAny<ProductSqlModel>()), Times.Exactly(1));
        }
    }
}