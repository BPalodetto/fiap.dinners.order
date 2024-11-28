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
            CustomerSqlModel customer = new CustomerSqlModel()
            {
                Name = "Alex",
                Email = "alex@email.com",
                Cpf = "333.824.233-67",
                Id = 938
            };

            _customerSqlRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<CustomerSqlModel, bool>>>(),
                                                         It.IsAny<bool>(),
                                                         default)).ReturnsAsync(customer);

            var result = await _customerRepositoryAdapter.GetByCpf("333.824.233-67", default);

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
        public async void DevePermitirObterCliente()
        {
            CustomerSqlModel customer = new CustomerSqlModel()
            {
                Name = "Alex",
                Email = "alex@email.com",
                Cpf = "333.824.233-67",
                Id = 938
            };

            _customerSqlRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<CustomerSqlModel, bool>>>(),
                                                         It.IsAny<bool>(),
                                                         default)).ReturnsAsync(customer);

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
            CustomerSqlModel customerSqlModel = new CustomerSqlModel()
            {
                Name = "Alex",
                Email = "alex@email.com",
                Cpf = "333.824.233-67",
                Id = 938
            };

            Customer customer = new Customer()
            {
                Name = "Alex",
                Email = "alex@email.com",
                Cpf = "333.824.233-67",
                Id = 938
            };

            _customerSqlRepository.Setup(x => x.Add(It.IsAny<CustomerSqlModel>())).Returns(customerSqlModel);
            _customerSqlRepository.Setup(x => x.UnitOfWork.CommitAsync(default)).ReturnsAsync(1);

            var result = await _customerRepositoryAdapter.CreateAsync(customer, default);

            Assert.NotNull(customer);
            Assert.NotNull(customer);
            Assert.Contains(result.Name, customer.Name);
            Assert.Contains(result.Email, customer.Email);
            Assert.Contains(result.Cpf, customer.Cpf);
            Assert.True(result.Id == customer.Id);

            _customerSqlRepository.Verify(p => p.Add(It.IsAny<CustomerSqlModel>()), Times.Exactly(1));
        }

        [Fact]
        public async void DevePermitirClienteValido()
        {
            _customerSqlRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<CustomerSqlModel, bool>>>(),
                                                           default)
                                                           ).ReturnsAsync(1);

            var result = await _customerRepositoryAdapter.ExistsByCpf("333.824.233-67", default);

            Assert.True(result);

            _customerSqlRepository.Verify(p => p.CountAsync(It.IsAny<Expression<Func<CustomerSqlModel, bool>>>(),
                                                           default), Times.Exactly(1));
        }
    }
}