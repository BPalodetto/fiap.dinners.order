using Core.Notifications;
using Domain.Entities.CustomerAggregate;
using Domain.Repositories;
using Domain.ValueObjects;
using Moq;
using UseCase.Dtos.CustomerRequest;
using UseCase.Services;
using Xunit.Sdk;

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
            
        }

        [Fact]
        public async void DevePermitirObterClientePorCpf()
        {
            customerMockResponse = new Customer
            {
                Cpf = "483.617.217-97",
                Name = "Alex",
                Email = "alex@email.com",
                Id = 938
            };

            _customerRepository.Setup(x => x.GetByCpf(It.IsAny<Cpf>(), default)).ReturnsAsync(customerMockResponse);         

            var result = await _customerUseCase.GetByCpf("483.617.217-97", default);

            Assert.NotNull(result);
            Assert.Contains(result.Name, customerMockResponse.Name);
            Assert.Contains(result.Email, customerMockResponse.Email);
            Assert.Contains(result.Cpf, customerMockResponse.Cpf);
            _customerRepository.Verify(p => p.GetByCpf(It.IsAny<Cpf>(), default), Times.Exactly(1));
        }

        [Fact]
        public async void DevePermitirCriarCliente()
        {
            CreateCustomerRequest createCustomerRequest = new CreateCustomerRequest
            {
                Cpf = "483.617.217-97",
                Name = "Alex",
                Email = "alex@email.com",
            };

            customerMockResponse = new Customer
            {
                Cpf = "483.617.217-97",
                Name = "Alex",
                Email = "alex@email.com",
                Id = 938
            };

            _customerRepository.Setup(x => x.ExistsByCpf(It.IsAny<Cpf>(), default)).ReturnsAsync(false);
            _customerRepository.Setup(x => x.CreateAsync(It.IsAny<Customer>(), default)).ReturnsAsync(customerMockResponse);

            var result = await _customerUseCase.CreateAsync(createCustomerRequest, default);

            Assert.NotNull(result);
            Assert.Contains(result.Name, customerMockResponse.Name);
            Assert.Contains(result.Email, customerMockResponse.Email);
            Assert.Contains(result.Cpf, customerMockResponse.Cpf);
            _customerRepository.Verify(p => p.ExistsByCpf(It.IsAny<Cpf>(), default), Times.Exactly(1));
        }
    }
}