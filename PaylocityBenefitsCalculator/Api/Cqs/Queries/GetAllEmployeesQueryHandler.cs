using Api.Data.Contracts;
using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using MediatR;

namespace Api.Cqs.Queries
{
	public class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, ApiResponse<List<GetEmployeeDto>>>
	{
		/**
		 * Creating a constant string for the error message that will be used in the event of an exception.
		 * 
		 * Marking this as internal so that we can leverage the CSPROJ InternalsVisibleTo attribute to allow the unit tests to access this constant.
		 */
		internal const string ERR_MSG_GET_EMPLOYEES = "Error getting all employees";

		private readonly IEmployeeRepository _repository;
		private readonly ILogger<GetAllEmployeesQueryHandler> _logger;

		public GetAllEmployeesQueryHandler(IEmployeeRepository repo, ILogger<GetAllEmployeesQueryHandler> logger)
		{
			_repository = repo ?? throw new ArgumentNullException(nameof(repo));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<ApiResponse<List<GetEmployeeDto>>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
		{
			try
			{
				// Adding debug logging so that in the event of an application failure, we can see the steps that were taken.
				_logger.LogDebug("Calling repository to get all employees");
				var employeeQuery = await _repository.GetAllAsync(cancellationToken);

				_logger.LogDebug("Converting employees to DTOs");
				var employees = employeeQuery.Select(e => new GetEmployeeDto
				{
					Id = e.Id,
					FirstName = e.FirstName,
					LastName = e.LastName,
					Salary = e.Salary,
					DateOfBirth = e.DateOfBirth,
					Dependents = e.Dependents.Select(d => new GetDependentDto
					{
						Id = d.Id,
						FirstName = d.FirstName,
						LastName = d.LastName,
						DateOfBirth = d.DateOfBirth,
						Relationship = d.Relationship
					}).ToList()
				}).ToList();

				return new ApiResponse<List<GetEmployeeDto>>
				{
					Success = true,
					Data = employees
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ERR_MSG_GET_EMPLOYEES);
				return new ApiResponse<List<GetEmployeeDto>>
				{
					Success = false,
					Error = ERR_MSG_GET_EMPLOYEES
				};
			}
		}
	}
}
