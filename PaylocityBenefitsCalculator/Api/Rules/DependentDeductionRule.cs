using Api.Models;

namespace Api.Rules
{
	/// <summary>
	/// Rule that handles the deduction per dependent
	/// </summary>
	public class DependentDeductionRule : IDeductionRule
	{
		public decimal Calculate(Employee employee)
		{
			if (employee is null) ArgumentNullException.ThrowIfNull(employee);
			if (!employee.Dependents.Any()) return 0;

			return employee.Dependents.Count * 600;
		}
	}
}
