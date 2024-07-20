using Api.Dtos.Employee;
using Api.Models;
using MediatR;

namespace Api.Cqs.Queries
{
	public class GetAllEmployeesQuery : IRequest<ApiResponse<List<GetEmployeeDto>>>
	{
	}
}
