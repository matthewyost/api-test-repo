using Api.Data.Contracts;
using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using MediatR;

namespace Api.Cqs.Queries
{
	public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, ApiResponse<GetEmployeeDto>>
	{
		internal const string ERR_MSG_EMPLOYEE_NOT_FOUND = "Employee not found.";
		internal const string ERR_MSG_GET_EMPLOYEE = "Error getting employee.";
		internal const string ERR_MSG_GET_EMPLOYEE_ID = "Employee with id {employeeId} not found.";
		internal const string ERR_MSG_GET_EMPLOYEE_ID_EX = "Error getting employee with id {employeeId}.";

		private readonly ILogger<GetEmployeeByIdQueryHandler> _logger;
		private readonly IEmployeeRepository _repository;

		public GetEmployeeByIdQueryHandler(ILogger<GetEmployeeByIdQueryHandler> logger, IEmployeeRepository repo)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_repository = repo ?? throw new ArgumentNullException(nameof(repo));
		}

		public async Task<ApiResponse<GetEmployeeDto>> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
		{
			try
			{
				// Adding debug logging so that in the event of an application failure, we can see the steps that were taken.
				_logger.LogDebug("Calling repository to get an employee for {id}", request.Id);
				var employeeModel = await _repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

				if (employeeModel is null)
				{
					_logger.LogWarning(ERR_MSG_GET_EMPLOYEE_ID, request.Id);
					return new ApiResponse<GetEmployeeDto>
					{
						Success = true
					};
				}

				_logger.LogDebug("Converting employee to DTO");
				var employee = new GetEmployeeDto
				{
					Id = employeeModel.Id,
					FirstName = employeeModel.FirstName,
					LastName = employeeModel.LastName,
					Salary = employeeModel.Salary,
					DateOfBirth = employeeModel.DateOfBirth,
					Dependents = employeeModel.Dependents.Select(d => new GetDependentDto
					{
						Id = d.Id,
						FirstName = d.FirstName,
						LastName = d.LastName,
						DateOfBirth = d.DateOfBirth,
						Relationship = d.Relationship
					}).ToList()
				};

				return new ApiResponse<GetEmployeeDto>
				{
					Success = true,
					Data = employee
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ERR_MSG_GET_EMPLOYEE_ID_EX, request.Id);
				return new ApiResponse<GetEmployeeDto>
				{
					Success = false,
					Error = ERR_MSG_GET_EMPLOYEE
				};
			}
		}
	}
}
