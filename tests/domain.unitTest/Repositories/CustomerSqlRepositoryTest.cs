using Infrastructure.Context;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Interfaces;
using Infrastructure.SqlModels.CustomerAggregate;
using Moq;
using System.Linq.Expressions;

namespace Repository.CustomerSqlTest
{
    public class CustomerSqlRepositoryTest
    {
        CustomerSqlRepository _customerRepository;

        Mock<DinersOrderSqlContext> _context;
        Mock<ICustomerSqlRepository> _customerSqlRepository;
        public CustomerSqlRepositoryTest()
        {
            _context = new Mock<DinersOrderSqlContext>();
            _customerSqlRepository = new Mock<ICustomerSqlRepository>();

            _customerRepository = new CustomerSqlRepository(_context.Object);
        }

        [Fact]
        public void DevePermitirAdicionarCliente()
        {
            CustomerSqlModel customerSqlModel = new CustomerSqlModel()
            {
                Name = "Alex",
                Email = "alex@email.com",
                Cpf = "333.824.233-67",
                CreatedAt = DateTime.Now,
                Id = 938,
                UpdatedAt = DateTime.Now
            };

            _context.Setup(x => x.Add(It.IsAny<CustomerSqlModel>()).Entity).Returns(customerSqlModel);

            _customerRepository.Add(customerSqlModel);

            Assert.NotNull(customerSqlModel);
            _context.Verify(p => p.Add(It.IsAny<CustomerSqlModel>()).Entity, Times.Exactly(1));
        }

        [Fact]
        public async void DevePermitirObterClientePorCpf()
        {
            CustomerSqlModel customerSqlModel = new CustomerSqlModel()
            {
                Name = "Alex",
                Email = "alex@email.com",
                Cpf = "333.824.233-67",
                CreatedAt = DateTime.Now,
                Id = 938,
                UpdatedAt = DateTime.Now
            };

            _customerSqlRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<CustomerSqlModel, bool>>>(),
                                                        It.IsAny<bool>(),
                                                        default)).ReturnsAsync(customerSqlModel);

            var result = await _customerRepository.GetAsync(x => x.Cpf.Equals("333.824.233-67"), false, default);

            Assert.NotNull(customerSqlModel);
            _customerSqlRepository.Verify(p => p.GetAsync(It.IsAny<Expression<Func<CustomerSqlModel, bool>>>(),
                                                        It.IsAny<bool>(),
                                                        default), Times.Exactly(1));
        }

        [Fact]
        public async void DevePermitirObterClientePorId()
        {
            CustomerSqlModel customerSqlModel = new CustomerSqlModel()
            {
                Name = "Alex",
                Email = "alex@email.com",
                Cpf = "333.824.233-67",
                CreatedAt = DateTime.Now,
                Id = 938,
                UpdatedAt = DateTime.Now
            };

            _customerSqlRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<CustomerSqlModel, bool>>>(),
                                                        It.IsAny<bool>(),
                                                        default)).ReturnsAsync(customerSqlModel);

            var result = await _customerRepository.GetAsync(x => x.Id.Equals(938), false, default);

            Assert.NotNull(customerSqlModel);
            _customerSqlRepository.Verify(p => p.GetAsync(It.IsAny<Expression<Func<CustomerSqlModel, bool>>>(),
                                                        It.IsAny<bool>(),
                                                        default), Times.Exactly(1));
        }

        [Fact]
        public async void DevePermitirContarCliente()
        {
            _customerSqlRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<CustomerSqlModel, bool>>>(), default)).ReturnsAsync(1);

            var result = await _customerRepository.CountAsync(x => x.Cpf.Equals("333.824.233-67"), default);

            Assert.True(result > 0);
            Assert.Equal(1, result);
            _customerSqlRepository.Verify(p => p.CountAsync(It.IsAny<Expression<Func<CustomerSqlModel, bool>>>(), default), Times.Exactly(1));
        }
    }
}