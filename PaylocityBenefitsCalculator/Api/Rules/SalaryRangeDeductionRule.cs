using Api.Models;

namespace Api.Rules
{
	/// <summary>
	/// Employees that make more than $80,000/year will incur an additional
	/// 2% of their yearly salary in benefits costs
	/// </summary>
	public class SalaryRangeDeductionRule : IDeductionRule
	{
		public decimal Calculate(Employee employee)
		{
			if (employee is null) ArgumentNullException.ThrowIfNull(employee);

			if (employee.Salary > 80000)
			{
				return (employee.Salary * 0.02m) / 26;
			}

			return 0;
		}
	}
}
