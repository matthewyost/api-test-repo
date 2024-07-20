using Api.Data.Contracts;
using Api.Dtos.Dependent;
using Api.Models;
using MediatR;

namespace Api.Cqs.Queries
{
	/// <summary>
	/// Handler for GetAllDependentsQuery
	/// </summary>
	public class GetAllDependentsQueryHandler : IRequestHandler<GetAllDependentsQuery, ApiResponse<List<GetDependentDto>>>
	{
		internal const string ERR_MSG_GET_DEPENDENTS = "Error getting all dependents";

		private readonly IDependentRepository _repository;
		private readonly ILogger<GetAllDependentsQueryHandler> _logger;

		/// <summary>
		/// Constructor for <see cref="GetAllDependentsQueryHandler"/>
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="repo"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public GetAllDependentsQueryHandler(ILogger<GetAllDependentsQueryHandler> logger, IDependentRepository repo)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_repository = repo ?? throw new ArgumentNullException(nameof(repo));
		}

		/// <inheritdoc />
		public async Task<ApiResponse<List<GetDependentDto>>> Handle(GetAllDependentsQuery request, CancellationToken cancellationToken)
		{
			try
			{

				_logger.LogDebug("Calling repository to get all dependents");
				var dependents = await _repository.GetAllAsync(cancellationToken);

				_logger.LogDebug("Converting dependents to DTOs");
				var dependentDtos = dependents.Select(d => new GetDependentDto
				{
					Id = d.Id,
					FirstName = d.FirstName,
					LastName = d.LastName,
					DateOfBirth = d.DateOfBirth,
					Relationship = d.Relationship
				}).ToList();

				return new ApiResponse<List<GetDependentDto>>
				{
					Success = true,
					Data = dependentDtos
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ERR_MSG_GET_DEPENDENTS);
				return new ApiResponse<List<GetDependentDto>>
				{
					Success = false,
					Error = ERR_MSG_GET_DEPENDENTS
				};
			}
		}
	}
}
