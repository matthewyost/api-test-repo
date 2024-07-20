namespace Api.Rules
{
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		/// Adds all the deduction rules to the DI container
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddRules(this IServiceCollection services)
		{
			typeof(ServiceCollectionExtensions).Assembly.GetTypes()
				.Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Contains(typeof(IDeductionRule)))
				.ToList()
				.ForEach(t => services.AddSingleton(typeof(IDeductionRule), t));

			services.AddSingleton<DeductionRuleEngine>();

			return services;
		}
	}
}
