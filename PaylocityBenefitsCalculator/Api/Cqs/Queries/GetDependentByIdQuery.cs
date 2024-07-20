using Api.Dtos.Dependent;
using Api.Models;
using MediatR;

namespace Api.Cqs.Queries
{
	/// <summary>
	/// Query to get a dependent by id
	/// </summary>
	public class GetDependentByIdQuery : IRequest<ApiResponse<GetDependentDto>>
	{
		/// <summary>
		/// Constructor for GetDependentByIdQuery
		/// </summary>
		/// <param name="id"></param>
		public GetDependentByIdQuery(Guid id)
		{
			Id = id;
		}

		/// <summary>
		/// Id of the dependent to get
		/// </summary>
		/// <remarks>
		/// Changed data type from int to Guid for security reasons.
		/// </remarks>
		public Guid Id { get; init; }
	}
}
