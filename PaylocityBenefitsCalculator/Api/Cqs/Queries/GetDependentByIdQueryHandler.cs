using Api.Data.Contracts;
using Api.Dtos.Dependent;
using Api.Models;
using MediatR;

namespace Api.Cqs.Queries
{
	/// <summary>
	/// Handler for the <see cref="GetDependentByIdQuery"/>.
	/// </summary>
	public class GetDependentByIdQueryHandler : IRequestHandler<GetDependentByIdQuery, ApiResponse<GetDependentDto>>
	{
		internal const string ERR_MSG_DEPENDENT_NOT_FOUND = "Dependent not found.";
		internal const string ERR_MSG_GET_DEPENDENT = "Error getting dependent.";
		internal const string ERR_MSG_GET_DEPENDENT_ID = "Dependent with id {requestId} not found.";
		internal const string ERR_MSG_GET_DEPENDENT_ID_EX = "Error getting dependent with id {requestId}.";

		private readonly IDependentRepository _repository;
		private readonly ILogger<GetDependentByIdQueryHandler> _logger;

		/// <summary>
		/// Initializes a new instance of the <see cref="GetDependentByIdQueryHandler"/> class.
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="repo"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public GetDependentByIdQueryHandler(ILogger<GetDependentByIdQueryHandler> logger, IDependentRepository repo)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_repository = repo ?? throw new ArgumentNullException(nameof(repo));
		}

		/// <inheritdoc />
		public async Task<ApiResponse<GetDependentDto>> Handle(GetDependentByIdQuery request, CancellationToken cancellationToken)
		{
			try
			{
				var dependent = await _repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

				if (dependent == null)
				{
					_logger.LogWarning(ERR_MSG_GET_DEPENDENT_ID, request.Id);
					// Returning success true here but returning the dependent as null
					return new ApiResponse<GetDependentDto>
					{
						Success = true
					};
				}

				return new ApiResponse<GetDependentDto>
				{
					Success = true,
					Data = new GetDependentDto
					{
						Id = dependent.Id,
						FirstName = dependent.FirstName,
						LastName = dependent.LastName,
						Relationship = dependent.Relationship,
						DateOfBirth = dependent.DateOfBirth
					}
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ERR_MSG_GET_DEPENDENT_ID_EX, request.Id);
				return new ApiResponse<GetDependentDto>
				{
					Success = false,
					Error = ERR_MSG_GET_DEPENDENT
				};
			}
		}
	}
}
