using Api.Data.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Testcontainers.MsSql;
using Xunit;

namespace ApiTests.Utilities
{
	/// <summary>
	/// Test fixture that will host the web application factory that can be reused 
	/// between any test classes that are part of the collection.  This fixture can
	/// also replace any services with mock services for testing purposes.
	/// </summary>
	public class ApiWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
	{
		private readonly MsSqlContainer container = new MsSqlBuilder()
			.WithImage("mcr.microsoft.com/mssql/server:2022-latest")
			.WithPassword("yourStrong(!)Password")
			.Build();

		public ApiWebApplicationFactory()
		{
		}

		public Task InitializeAsync() => container.StartAsync();

		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			builder.ConfigureTestServices(services =>
			{
				// Got to remove the existing DataContext and replace it with a new one
				var descriptor = services.SingleOrDefault(
					d => d.ServiceType ==
						typeof(DbContextOptions<DataContext>));
				if (descriptor is not null) services.Remove(descriptor);

				services.AddDbContext<DataContext>(options =>
				{
					options.UseSqlServer(container.GetConnectionString());
				});

				// Make sure the database is created and seeded
				var serviceProvider = services.BuildServiceProvider();

				using var scope = serviceProvider.CreateScope();
				var scopedServices = scope.ServiceProvider;
				var context = scopedServices.GetRequiredService<DataContext>();
				context.Database.EnsureCreated();
			});

			base.ConfigureWebHost(builder);
		}

		Task IAsyncLifetime.DisposeAsync() => container.DisposeAsync().AsTask();
	}
}
