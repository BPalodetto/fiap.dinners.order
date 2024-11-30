using Domain.Entities.Enums;
using Domain.Entities.ProductAggregate;
using Infrastructure.Adapters;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Interfaces;
using Infrastructure.SqlModels.ProductAggregate;
using Moq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Adapters.ProductRepositoryTest
{
    public class ProductRepositoryAdapterTest
    {
        ProductRepositoryAdapter _productRepositoryAdapter;
        Mock<IProductSqlRepository> _productSQLRepository;
        public ProductRepositoryAdapterTest()
        {
            _productSQLRepository = new Mock<IProductSqlRepository>();

            _productRepositoryAdapter = new ProductRepositoryAdapter(_productSQLRepository.Object);
        }

        [Fact]
        public async void DevePermitirObterProduto()
        {
            ProductSqlModel productSqlModel = new ProductSqlModel()
            {
                Id = 1,
                Name = "X-Salada",
                Price = 20,
                ProductType = Domain.Entities.Enums.ProductType.Snack,
                PhotoContentType = string.Empty,
                Description = "Fiap Hamburguer",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            _productSQLRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<ProductSqlModel, bool>>>(),
                                                        It.IsAny<bool>(),
                                                        default)).ReturnsAsync(productSqlModel);

            var result = await _productRepositoryAdapter.GetAsync(1, default);

            Assert.NotNull(result);
            Assert.True(result.Id == productSqlModel.Id);
            Assert.Contains(result.Name, productSqlModel.Name);
            Assert.True(result.Price == productSqlModel.Price);
            _productSQLRepository.Verify(p => p.GetAsync(It.IsAny<Expression<Func<ProductSqlModel, bool>>>(),
                                                        It.IsAny<bool>(),
                                                        default), Times.Exactly(1));

        }

        [Fact]
        public async void DevePermitirListarProduto()
        {
            List<ProductSqlModel> products = new List<ProductSqlModel>()
            {
                new ProductSqlModel
                {

                    Id = 1,
                    Name = "X-Salada",
                    Price = 20,
                    ProductType = Domain.Entities.Enums.ProductType.Snack,
                    PhotoContentType = string.Empty,
                    Description = "Fiap Hamburguer",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                }
            };

            _productSQLRepository.Setup(x => x.ListAsync(It.IsAny<Expression<Func<ProductSqlModel, bool>>>(),
                                                         It.IsAny<int>(),
                                                         It.IsAny<int>(),
                                                         default)).ReturnsAsync(products);

           var result = await _productRepositoryAdapter.ListAsync(Domain.Entities.Enums.ProductType.Snack, 1, 10, default);

            Assert.NotNull(result);
            Assert.True(result.Any());
            _productSQLRepository.Verify(x => x.ListAsync(It.IsAny<Expression<Func<ProductSqlModel, bool>>>(),
                                                         It.IsAny<int>(),
                                                         It.IsAny<int>(),
                                                         default), Times.Exactly(1));
        }

        [Fact]
        public async void DevePermitirCriarProduto()
        {
            Product product = new() 
            {
                Description = "Fiap Hamburguer", 
                Name = "X-Salada", 
                Price = 20,
                ProductType = ProductType.Snack
            };

            _productSQLRepository.Setup(x => x.Add(It.IsAny<ProductSqlModel>()));
            _productSQLRepository.Setup(x => x.UnitOfWork.CommitAsync(default)).ReturnsAsync(1);

            var result = await _productRepositoryAdapter.CreateAsync(product, default);

            Assert.NotNull(result);
            Assert.Contains(result.Description, product.Description);
            Assert.Contains(result.Name, product.Name);
            Assert.True(result.Price == product.Price);
            Assert.True(result.ProductType == product.ProductType);
        }

        [Fact]
        public async void DevePermitirAtualizarProduto()
        {
            Product product = new()
            {
                Description = "Fiap Hamburguer",
                Name = "X-Salada",
                Price = 20,
                ProductType = ProductType.Snack
            };

            ProductSqlModel productSqlModel = new ProductSqlModel()
            {
                Id = 1,
                Name = "X-Salada",
                Price = 20,
                ProductType = Domain.Entities.Enums.ProductType.Snack,
                PhotoContentType = string.Empty,
                Description = "Fiap Hamburguer",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            _productSQLRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<ProductSqlModel, bool>>>(),
                                                        It.IsAny<bool>(),
                                                        default)).ReturnsAsync(productSqlModel);
            _productSQLRepository.Setup(x => x.Update(It.IsAny<ProductSqlModel>()));
            _productSQLRepository.Setup(x => x.UnitOfWork.CommitAsync(default)).ReturnsAsync(1);

            await _productRepositoryAdapter.UpdateAsync(product, default);
        }
    }
}