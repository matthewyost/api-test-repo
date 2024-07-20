using Api.Data.Contracts;

namespace Api.Data
{
	public static class ServiceCollectionExtensions
	{

		public static IServiceCollection AddRepositories(this IServiceCollection services)
		{
			services.AddScoped<IEmployeeRepository, EmployeeRepository>();
			services.AddScoped<IDependentRepository, DependentRepository>();

			return services;
		}
	}
}
