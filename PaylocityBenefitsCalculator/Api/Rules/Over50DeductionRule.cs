using Api.Models;

namespace Api.Rules
{
	/// <summary>
	/// Any dependents that are over the age of 50 years old
	/// will incur an additional $200/month in benefits costs
	/// </summary>
	public class Over50DeductionRule : IDeductionRule
	{
		public decimal Calculate(Employee employee)
		{
			if (employee is null) ArgumentNullException.ThrowIfNull(employee);

			decimal deduction = 0;

			if (employee is not null && employee.Dependents is not null && employee.Dependents.Any())
			{
				foreach (Dependent dependent in employee.Dependents)
				{
					if (IsOver50(dependent))
						deduction += 200;
				}
			}

			return deduction;
		}

		/// <summary>
		/// Determine if a dependent is over 50 years old
		/// </summary>
		/// <param name="dependent"></param>
		/// <returns></returns>
		/// <remarks>
		/// While this is not a perfect way to determine if a dependent is over 50 years old,
		/// this will suffice for the purposes of this exercise.
		/// </remarks>
		private static bool IsOver50(Dependent dependent)
		{
			return (DateTime.UtcNow.Year - dependent.DateOfBirth.Year) > 50;
		}
	}
}
