using Api.Dtos.Employee;
using Api.Models;
using MediatR;

namespace Api.Cqs.Queries;

/// <summary>
/// Query for retrieving an employee by it's id
/// </summary>
public class GetEmployeeByIdQuery : IRequest<ApiResponse<GetEmployeeDto>>
{
	/// <summary>
	/// Constructor for <see cref="GetEmployeeByIdQuery"/>
	/// </summary>
	/// <param name="id"></param>
	public GetEmployeeByIdQuery(Guid id)
	{
		Id = id;
	}

	/// <summary>
	/// Employee Id
	/// </summary>
	/// <remarks>
	/// Changed from int for security as a number can
	/// be too easily guessed leading to security issues.
	/// </remarks>
	public Guid Id { get; init; }
}
