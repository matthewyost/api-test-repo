using Api.Cqs.Queries;
using Api.Dtos.Employee;
using Api.Dtos.Paycheck;
using Api.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

/// <summary>
/// Controller for employees
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController : ControllerBase
{
	private readonly IMediator _mediator;

	/// <summary>
	/// Constructor for <see cref="EmployeesController"/>
	/// </summary>
	/// <param name="mediator"></param>
	/// <exception cref="ArgumentNullException"></exception>
	public EmployeesController(IMediator mediator)
	{
		_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
	}


	[SwaggerOperation(Summary = "Get employee by id")]
	[ProducesResponseType(200, Type = typeof(ApiResponse<GetEmployeeDto>))]
	[ProducesResponseType(400, Type = typeof(ApiResponse<GetEmployeeDto>))]
	[ProducesResponseType(401, Type = typeof(ApiResponse<GetEmployeeDto>))] // Could be used if authentication is required
	[ProducesResponseType(403, Type = typeof(ApiResponse<GetEmployeeDto>))] // Could be used if authorization is required
	[ProducesResponseType(404, Type = typeof(ApiResponse<GetEmployeeDto>))]
	[ProducesResponseType(429, Type = typeof(ApiResponse<GetEmployeeDto>))] // Could be used for rate limiting
	[ProducesResponseType(500, Type = typeof(ApiResponse<GetEmployeeDto>))]
	[Produces("application/json")]
	[HttpGet("{employeeId}")]
	public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(Guid employeeId)
	{
		var response = await _mediator.Send(new GetEmployeeByIdQuery(employeeId));
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

	[SwaggerOperation(Summary = "Get paycheck for employee by id")]
	[ProducesResponseType(200, Type = typeof(ApiResponse<PaycheckDto>))]
	[ProducesResponseType(400, Type = typeof(ApiResponse<PaycheckDto>))]
	[ProducesResponseType(401, Type = typeof(ApiResponse<PaycheckDto>))] // Could be used if authentication is required
	[ProducesResponseType(403, Type = typeof(ApiResponse<PaycheckDto>))] // Could be used if authorization is required
	[ProducesResponseType(404, Type = typeof(ApiResponse<PaycheckDto>))]
	[ProducesResponseType(429, Type = typeof(ApiResponse<PaycheckDto>))] // Could be used for rate limiting
	[ProducesResponseType(500, Type = typeof(ApiResponse<PaycheckDto>))]
	[Produces("application/json")]
	[HttpGet("{employeeId}/paycheck")]
	public async Task<ActionResult<ApiResponse<PaycheckDto>>> GetPaycheck(Guid employeeId)
	{
		var response = await _mediator.Send(new GetPaycheckQuery(employeeId));
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

	[SwaggerOperation(Summary = "Get all employees")]
	[ProducesResponseType(200, Type = typeof(ApiResponse<List<GetEmployeeDto>>))]
	[ProducesResponseType(401, Type = typeof(ApiResponse<List<GetEmployeeDto>>))] // Could be used if authentication is required
	[ProducesResponseType(403, Type = typeof(ApiResponse<List<GetEmployeeDto>>))] // Could be used if authorization is required
	[ProducesResponseType(429, Type = typeof(ApiResponse<List<GetEmployeeDto>>))] // Could be used for rate limiting
	[ProducesResponseType(500, Type = typeof(ApiResponse<List<GetEmployeeDto>>))]
	[HttpGet("")]
	public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
	{
		var response = await _mediator.Send(new GetAllEmployeesQuery());
		if (response.Success)
			return Ok(response);
		else
			return BadRequest(response);

	}
}
