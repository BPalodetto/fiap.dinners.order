using Domain.Entities.CustomerAggregate;
using Domain.Repositories;
using Domain.ValueObjects;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Interfaces;
using Infrastructure.SqlModels.CustomerAggregate;
using Moq;
using System.Linq.Expressions;

namespace Adapters.CustomerRepositoryTest
{
    public class CustomerRepositoryAdapterTest
    {
        CustomerRepositoryAdapter _customerRepositoryAdapter;

        Mock<ICustomerSqlRepository> _customerSqlRepository;

        Mock<ICustomerRepository> _customerRepository; 
        public CustomerRepositoryAdapterTest()
        {
            _customerSqlRepository = new Mock<ICustomerSqlRepository>();
            _customerRepository = new Mock<ICustomerRepository>();

            _customerRepositoryAdapter = new CustomerRepositoryAdapter(_customerSqlRepository.Object);
        }

        [Fact]
        public async void DevePermitirObterClientePorCpf()
        {
            Customer customer = new Customer()
            {
                Name = "Alex",
                Email = "alex@email.com",
                Cpf = "333.824.233-67",
                Id = 938
            };

            _customerRepository.Setup(x => x.GetByCpf(It.IsAny<Cpf>(), default)).ReturnsAsync(customer);

            var result = await _customerRepositoryAdapter.GetByCpf("333.824.233-67", default);

            Assert.NotNull(customer);
            Assert.Contains(result.Name, customer.Name);
            Assert.Contains(result.Email, customer.Email);
            Assert.Contains(result.Cpf, customer.Cpf);
            Assert.True(result.Id == customer.Id);
            _customerRepository.Verify(p => p.GetByCpf(It.IsAny<Cpf>(), default), Times.Exactly(1));
        }

        [Fact]
        public async void DevePermitirObterCliente()
        {
            Customer customer = new Customer()
            {
                Name = "Alex",
                Email = "alex@email.com",
                Cpf = "333.824.233-67",
                Id = 938
            };

            _customerRepository.Setup(x => x.GetAsync(It.IsAny<int>(), default)).ReturnsAsync(customer);

            var result = await _customerRepositoryAdapter.GetAsync(938, default);

            Assert.NotNull(customer);
            Assert.NotNull(customer);
            Assert.Contains(result.Name, customer.Name);
            Assert.Contains(result.Email, customer.Email);
            Assert.Contains(result.Cpf, customer.Cpf);
            Assert.True(result.Id == customer.Id);

            _customerSqlRepository.Verify(p => p.GetAsync(It.IsAny<Expression<Func<CustomerSqlModel, bool>>>(),
                                                        It.IsAny<bool>(),
                                                        default), Times.Exactly(1));
        }

        [Fact]
        public async void DevePermitirCriarCliente()
        {
            Customer customer = new Customer()
            {
                Name = "Alex",
                Email = "alex@email.com",
                Cpf = "333.824.233-67",
                Id = 938
            };

            _customerRepository.Setup(x => x.CreateAsync(It.IsAny<Customer>(), default)).ReturnsAsync(customer);

            var result = await _customerRepositoryAdapter.CreateAsync(customer, default);

            Assert.NotNull(customer);
            Assert.NotNull(customer);
            Assert.Contains(result.Name, customer.Name);
            Assert.Contains(result.Email, customer.Email);
            Assert.Contains(result.Cpf, customer.Cpf);
            Assert.True(result.Id == customer.Id);

            _customerSqlRepository.Verify(p => p.GetAsync(It.IsAny<Expression<Func<CustomerSqlModel, bool>>>(),
                                                        It.IsAny<bool>(),
                                                        default), Times.Exactly(1));
        }

        [Fact]
        public async void DevePermitirClienteValido()
        {
            _customerRepository.Setup(x => x.ExistsByCpf(It.IsAny<Cpf>(), default)).ReturnsAsync(true);

            var result = await _customerRepositoryAdapter.ExistsByCpf("333.824.233-67", default);

            Assert.True(result);

            _customerRepository.Verify(p => p.ExistsByCpf(It.IsAny<Cpf>(), default), Times.Exactly(1));
        }
    }
}