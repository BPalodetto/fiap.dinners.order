using Core.Notifications;
using Domain.Entities.Enums;
using Domain.Entities.ProductAggregate;
using Domain.Repositories;
using Domain.ValueObjects;
using Moq;
using UseCase.Dtos.ProductRequest;
using UseCase.Services;

namespace UseCase.ProductTest
{
    public class ProductUseCaseTest
    {
        ProductUseCase _orderUseCase;

        Mock<IProductRepository> _productRepository;
        Mock<NotificationContext> _notificationContext;
        Product productResponseMock;

        byte[] byteArray = null;
        public ProductUseCaseTest()
        {
            _productRepository = new Mock<IProductRepository>();
            _notificationContext = new Mock<NotificationContext>();

            _orderUseCase = new ProductUseCase(_productRepository.Object,
                                            _notificationContext.Object
                                            );

            productResponseMock = new()
            {
                Description = "Hamburguer",
                Name = "X-Bacon",
                ProductType = ProductType.SideDish,
                Price = 30
            };
        }

        [Fact]
        public async void DevePermitirObterProduct()
        {
            _productRepository.Setup(x => x.GetAsync(It.IsAny<int>(), default)).ReturnsAsync(productResponseMock);

            var result = await _orderUseCase.GetAsync(1, default);

            Assert.NotNull(result);
            Assert.Contains(result.Name, productResponseMock.Name);
            Assert.True(result.ProductType == productResponseMock.ProductType);
            Assert.True(result.Price > 0);
            Assert.True(result.Price == productResponseMock.Price);
            _productRepository.Verify(p => p.GetAsync(It.IsAny<int>(), default), Times.Exactly(1));
        }

        [Fact]
        public async void DevePermitirListarProduct()
        {
            List<Product> products = new List<Product>();
            products.Add(productResponseMock);

            _productRepository.Setup(x => 
                                    x.ListAsync(It.IsAny<ProductType>(),
                                    It.IsAny<int>(), 
                                    It.IsAny<int>(), 
                                    default)).ReturnsAsync(products);


            var result = await _orderUseCase.ListAsync(ProductType.SideDish, 1, 10, default);

            Assert.NotNull(result);
            Assert.True(result.Any());
            _productRepository.Verify(p => p.ListAsync(It.IsAny<ProductType>(), It.IsAny<int>(), It.IsAny<int>(), default), Times.Exactly(1));
        }

        [Fact]
        public async void DevePermitirCriarProduct()
        {
       
            ProductCreateRequest productCreateRequest = new ProductCreateRequest()
            {
                Description = "Hamburguer",
                Name = "X-Bacon",
                ProductType = ProductType.SideDish,
                Price = 30,
                Photo = new Domain.ValueObjects.Photo("perfil.png", "image/jpeg", byteArray)
            };

            _productRepository.Setup(x => x.CreateAsync(It.IsAny<Product>(), default)).ReturnsAsync(productResponseMock);

            var result = await _orderUseCase.CreateAsync(productCreateRequest, default);

            Assert.NotNull(result);
            Assert.Contains(result.Name, productCreateRequest.Name);
            Assert.Contains(result.Description, productCreateRequest.Description);
            Assert.True(result.Price > 0);
            Assert.True(result.Price == productCreateRequest.Price);
            _productRepository.Verify(p => p.CreateAsync(It.IsAny<Product>(), default), Times.Exactly(1));
        }

        [Fact]
        public async void DevePermitirAtualizarPrecoProduct()
        {
            ProductUpdatePriceRequest productUpdatePriceRequest = new ProductUpdatePriceRequest()
            {
                Price = 25,
            };

            _productRepository.Setup(x => x.GetAsync(It.IsAny<int>(), default)).ReturnsAsync(productResponseMock);
            _productRepository.Setup(x => x.UpdateAsync(It.IsAny<Product>(), default));

            await _orderUseCase.UpdatePriceAsync(1, productUpdatePriceRequest, default);
        }

        [Fact]
        public async void DevePermitirAtualizarFotoProduct()
        {
            _productRepository.Setup(x => x.GetAsync(It.IsAny<int>(), default)).ReturnsAsync(productResponseMock);
            _productRepository.Setup(x => x.UpdateAsync(It.IsAny<Product>(), default));

            await _orderUseCase.UpdatePhoto(1, new Photo("novaFotoPerfil.png", "image/jpeg", byteArray), default);
        }
    }
}