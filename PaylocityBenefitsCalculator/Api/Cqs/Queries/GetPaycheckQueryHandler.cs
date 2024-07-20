using Api.Data.Contracts;
using Api.Dtos.Paycheck;
using Api.Models;
using Api.Rules;
using MediatR;

namespace Api.Cqs.Queries
{
	public class GetPaycheckQueryHandler : IRequestHandler<GetPaycheckQuery, ApiResponse<PaycheckDto>>
	{
		private readonly IEmployeeRepository _employeeRepo;
		private readonly DeductionRuleEngine _ruleEngine;

		public GetPaycheckQueryHandler(IEmployeeRepository employeeRepo, DeductionRuleEngine ruleEngine)
		{
			_employeeRepo = employeeRepo ?? throw new ArgumentNullException(nameof(employeeRepo));
			_ruleEngine = ruleEngine ?? throw new ArgumentNullException(nameof(ruleEngine));
		}

		public async Task<ApiResponse<PaycheckDto>> Handle(GetPaycheckQuery request, CancellationToken cancellationToken)
		{
			// Get the employee from the repository
			var employee = await _employeeRepo.GetByIdAsync(request.EmployeeId, cancellationToken);

			if (employee is null)
			{
				return new ApiResponse<PaycheckDto>
				{
					Success = false,
					Error = "Employee not found"
				};
			}

			// Calculate the deductions from the salary of the employee
			var deductions = _ruleEngine.Calculate(employee);

			// Split the salary into 26 paychecks and then deduct the monthly deductions
			var paycheck = employee.Salary / 26 - deductions;

			// Return the paycheck
			return new ApiResponse<PaycheckDto>
			{
				Success = true,
				Data = new PaycheckDto
				{
					EmployeeId = employee.Id,
					FirstName = employee.FirstName,
					LastName = employee.LastName,
					DateOfBirth = employee.DateOfBirth,
					NetPay = paycheck
				}
			};
		}
	}
}
