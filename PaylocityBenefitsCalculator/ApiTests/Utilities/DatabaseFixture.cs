using System.Threading.Tasks;
using Testcontainers.MsSql;
using Xunit;

namespace ApiTests.Utilities
{
	/// <summary>
	/// This is an xUnit fixture that will host an instance of the
	/// MsSql TestContainers.NET image for use in our integration tests
	/// </summary>
	public class DatabaseFixture : IAsyncLifetime
	{
		private readonly MsSqlContainer container = new MsSqlBuilder()
			.WithImage("mcr.microsoft.com/mssql/server:2022-latest")
			.Build();

		/// <summary>
		/// The connection string to the database
		/// </summary>
		/// <remarks>
		/// This will be used by the setup logic on our WebApplicationFactory
		/// in order to make the database connection available to the application
		/// </remarks>
		public string ConnectionString => container.GetConnectionString();

		public string ContainerId => $"{container.Id}";

		public Task DisposeAsync() => container.DisposeAsync().AsTask();

		public Task InitializeAsync() => container.StartAsync();
	}
}
