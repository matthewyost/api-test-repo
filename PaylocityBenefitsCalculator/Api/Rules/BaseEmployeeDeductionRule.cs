using Api.Models;

namespace Api.Rules
{
	/// <summary>
	/// Each employee has a base cost of $1000/month for benefits
	/// </summary>
	public class BaseEmployeeDeductionRule : IDeductionRule
	{
		public decimal Calculate(Employee employee)
		{
			if (employee is null) ArgumentNullException.ThrowIfNull(employee);

			return 1000;
		}
	}
}
