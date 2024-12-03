using Domain.Clients;
using Domain.Repositories;
using Infrastructure.Adapters;
using Infrastructure.Clients.RabbbitMq;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace WebApi.Extensions;

internal static class InfrastructureExtension
{
	private static string ConnectionString;

	static InfrastructureExtension()
	{
		ConnectionString = GetConnectionString();
	}
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{

		return services
			.AddSqlRepositories()
			.AddAdapters()
			.AddContext(configuration)
			.AddClients(configuration)
			.AddRabbitMqConnectionFactory(configuration);
	}

	private static IServiceCollection AddSqlRepositories(this IServiceCollection services)
	{
		return services
			.AddScoped<IOrderProductRepository, OrderProductRepository>()
			.AddScoped<ICustomerSqlRepository, CustomerSqlRepository>()
			.AddScoped<IOrderSqlRepository, OrderSqlRepository>()
			.AddScoped<IProductSqlRepository, ProductSqlRepository>();
	}

	private static IServiceCollection AddAdapters(this IServiceCollection services)
	{
		return services
			.AddScoped<ICustomerRepository, CustomerRepositoryAdapter>()
			.AddScoped<IProductRepository, ProductRepositoryAdapter>()
			.AddScoped<IOrderRepository, OrderRepositoryAdpater>();
	}

	private static IServiceCollection AddContext(this IServiceCollection services, IConfiguration configuration)
	{
		return
			services
				.AddDbContext<DinersOrderSqlContext>(opts =>
					opts.UseSqlServer(ConnectionString));
	}

    private static IServiceCollection AddClients(this IServiceCollection services, IConfiguration configuration)
    {
		return
			services
				.AddSingleton<IPaymentClient, PaymentRabbitMqClient>()
				.AddSingleton<IProductionClient, ProductionRabbitMqClient>();
    }

    private static IServiceCollection AddRabbitMqConnectionFactory(this IServiceCollection services, IConfiguration configuration)
    {
        var hostName = Environment.GetEnvironmentVariable("RabbitMqHostName");
        var port = int.Parse(Environment.GetEnvironmentVariable("RabbitMqPort"));
        var user = Environment.GetEnvironmentVariable("RabbitMqUserName");
        var password = Environment.GetEnvironmentVariable("RabbitMqPassword");

        return
            services
                .AddSingleton<IConnectionFactory>(
                    new ConnectionFactory()
                    {
                        HostName = hostName,
                        Port = port,
                        UserName = user,
                        Password = password
                    }
                );
    }

    public static void MigrationInitialisation(this IApplicationBuilder app)
	{
		Console.WriteLine("Iniciando migration");
		try
		{
			using (var serviceScope = app.ApplicationServices.CreateScope())
			{
				var db = serviceScope.ServiceProvider.GetRequiredService<DinersOrderSqlContext>();

				if (db.Database.GetPendingMigrations().Any())
				{
					db.Database.Migrate();
				}
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
		}

		Console.WriteLine("Migration finalizada");
	}

	private static string GetConnectionString()
	{
		var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");

		if (!string.IsNullOrEmpty(connectionString))
		{
			return connectionString;
		}

		throw new Exception("Enviroment Variable DefaultConnection not found ");
	}

}