using Api.Models;

namespace Api.Rules
{
	/// <summary>
	/// Interface for deduction rules
	/// </summary>
	public interface IDeductionRule
	{
		/// <summary>
		/// Calculates the deduction based on the employee
		/// </summary>
		/// <param name="employee"></param>
		/// <returns></returns>
		decimal Calculate(Employee employee);
	}
}
