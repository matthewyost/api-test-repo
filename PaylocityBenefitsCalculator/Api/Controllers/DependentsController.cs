using Api.Cqs.Queries;
using Api.Dtos.Dependent;
using Api.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

/// <summary>
/// Controller for dependents
/// </summary>
/// <remarks>
///		 Removed all logic from the controller and moved it to the handler
///		 so that the controller is only responsible for handling requests
///		 and returning responses.
///		 
///		 This also makes the controller easier to test as it is now only
///		 a passthrough and we do not need to worry about trying to mock
///		 the ControllerContext or any other ASP.NET Core specific classes.
/// </remarks>
[ApiController]
[Route("api/v1/[controller]")]
public class DependentsController : ControllerBase
{
	private readonly IMediator _mediator;

	/// <summary>
	/// Constructor for DependentsController
	/// </summary>
	/// <param name="mediator"></param>
	/// <exception cref="ArgumentNullException"></exception>
	public DependentsController(IMediator mediator)
	{
		_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
	}

	/// <summary>
	/// Retrieve a dependent by id
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	[SwaggerOperation(Summary = "Get dependent by id")]
	[ProducesResponseType(200, Type = typeof(ApiResponse<GetDependentDto>))]
	[ProducesResponseType(400, Type = typeof(ApiResponse<GetDependentDto>))]
	[ProducesResponseType(401, Type = typeof(ApiResponse<GetDependentDto>))] // Could be used if authentication is required
	[ProducesResponseType(403, Type = typeof(ApiResponse<GetDependentDto>))] // Could be used if authorization is required
	[ProducesResponseType(404, Type = typeof(ApiResponse<GetDependentDto>))]
	[ProducesResponseType(429, Type = typeof(ApiResponse<GetDependentDto>))] // Could be used for rate limiting
	[ProducesResponseType(500, Type = typeof(ApiResponse<GetDependentDto>))]
	[Produces("application/json")]
	[HttpGet("{id}")]
	public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(Guid id)
	{
		var response = await _mediator.Send(new GetDependentByIdQuery(id));

		if (response.Success)
		{
			if (response.Data is not null)
				return Ok(response);
			else
				return NotFound(response);
		}
		else
		{
			return BadRequest(response);
		}
	}

	/// <summary>
	/// Retrieve all dependents
	/// </summary>
	/// <returns></returns>
	[SwaggerOperation(Summary = "Get all dependents")]
	[ProducesResponseType(200, Type = typeof(ApiResponse<List<GetDependentDto>>))]
	[ProducesResponseType(400, Type = typeof(ApiResponse<List<GetDependentDto>>))]
	[ProducesResponseType(401, Type = typeof(ApiResponse<List<GetDependentDto>>))] // Could be used if authentication is required
	[ProducesResponseType(403, Type = typeof(ApiResponse<List<GetDependentDto>>))] // Could be used if authorization is required
	[ProducesResponseType(429, Type = typeof(ApiResponse<List<GetDependentDto>>))] // Could be used for rate limiting
	[ProducesResponseType(500, Type = typeof(ApiResponse<List<GetDependentDto>>))]
	[Produces("application/json")]
	[HttpGet("")]
	public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll()
	{
		var response = await _mediator.Send(new GetAllDependentsQuery());

		if (response.Success)
			return Ok(response);
		else
			return BadRequest(response);
	}
}
