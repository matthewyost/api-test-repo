using Api.Dtos.Dependent;
using Api.Models;
using MediatR;

namespace Api.Cqs.Queries
{
	/// <summary>
	/// Query to get all dependents
	/// </summary>
	public class GetAllDependentsQuery : IRequest<ApiResponse<List<GetDependentDto>>>
	{
	}
}
