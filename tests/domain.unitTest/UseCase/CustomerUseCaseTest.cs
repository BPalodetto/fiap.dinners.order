using Core.Notifications;
using Domain.Entities.CustomerAggregate;
using Domain.Repositories;
using Domain.ValueObjects;
using Moq;
using UseCase.Dtos.CustomerRequest;
using UseCase.Services;

namespace UseCase.CustomerTest
{
    public class CustomerUseCaseTest
    {
        CustomerUseCase _customerUseCase;

        Mock<ICustomerRepository> _customerRepository;
        Mock<NotificationContext> _notificationContext;

        Customer customerMockResponse;
        public CustomerUseCaseTest()
        {
            _customerRepository = new Mock<ICustomerRepository>();
            _notificationContext = new Mock<NotificationContext>();

            _customerUseCase = new CustomerUseCase(_customerRepository.Object,
                                            _notificationContext.Object
                                            );


            customerMockResponse = new Customer
            {
                Cpf = "333.824.233-67",
                Name = "Alex",
                Email = "alex@email.com",
                Id = 938
            };
        }

        [Fact]
        public async void DevePermitirObterClientePorCpf()
        {

            _customerRepository.Setup(x => x.GetByCpf(It.IsAny<string>(), default)).ReturnsAsync(customerMockResponse);

            var result = await _customerUseCase.GetByCpf("452.192.450-57", default);

            Assert.NotNull(result);
            Assert.Contains(result.Name, customerMockResponse.Name);
            Assert.Contains(result.Email, customerMockResponse.Email);
            Assert.Contains(result.Cpf, customerMockResponse.Cpf);
            _customerRepository.Verify(p => p.GetByCpf(It.IsAny<string>(), default), Times.Exactly(1));
        }

        [Fact]
        public async void DevePermitirCriarCliente()
        {
            CreateCustomerRequest createCustomerRequest = new CreateCustomerRequest
            {
                Cpf = "452.192.450-57",
                Name = "Alex",
                Email = "alex@email.com",
            };

            _customerRepository.Setup(x => x.ExistsByCpf(It.IsAny<Cpf>(), default)).ReturnsAsync(false);
            _customerRepository.Setup(x => x.CreateAsync(It.IsAny<Customer>(), default)).ReturnsAsync(customerMockResponse);

            var result = await _customerUseCase.CreateAsync(createCustomerRequest, default);

            Assert.NotNull(result);
            Assert.Contains(result.Name, customerMockResponse.Name);
            Assert.Contains(result.Email, customerMockResponse.Email);
            Assert.Contains(result.Cpf, customerMockResponse.Cpf);
            _customerRepository.Verify(p => p.GetByCpf(It.IsAny<string>(), default), Times.Exactly(2));
        }
    }
}