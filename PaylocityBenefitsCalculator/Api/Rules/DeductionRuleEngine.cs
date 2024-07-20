using Api.Models;

namespace Api.Rules
{
	public class DeductionRuleEngine
	{
		private readonly IEnumerable<IDeductionRule> _deductionRules;

		public DeductionRuleEngine(IEnumerable<IDeductionRule> deductionRules)
		{
			_deductionRules = deductionRules ?? throw new ArgumentNullException(nameof(deductionRules));
		}

		public decimal Calculate(Employee employee)
		{
			if (employee is null) ArgumentNullException.ThrowIfNull(employee);

			return _deductionRules.Sum(r => r.Calculate(employee));
		}
	}
}
