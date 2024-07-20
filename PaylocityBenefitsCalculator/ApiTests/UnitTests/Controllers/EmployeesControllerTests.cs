using Api.Controllers;
using Api.Cqs.Queries;
using Api.Dtos.Employee;
using Api.Dtos.Paycheck;
using Api.Models;
using Bogus;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ApiTests.UnitTests.Controllers
{
	public class EmployeesControllerTests
	{
		private readonly Faker<GetEmployeeDto> _faker = new();
		private readonly Mock<IMediator> _mediator = new();
		private readonly EmployeesController _controller;

		public EmployeesControllerTests()
		{
			_controller = new(_mediator.Object);
		}

		[Fact]
		public async Task GivenGetIsCalled_WhenIdIsValid_ThenReturnsEmployee()
		{
			// Arrange
			Guid id = Guid.NewGuid();
			GetEmployeeDto employee = _faker
				.RuleFor(d => d.Id, f => id)
				.Generate();
			_mediator.Setup(m => m.Send(It.IsAny<GetEmployeeByIdQuery>(), default))
				.ReturnsAsync(new ApiResponse<GetEmployeeDto>
				{
					Success = true,
					Data = employee
				});

			// Act
			ActionResult<ApiResponse<GetEmployeeDto>> result = await _controller.Get(id);

			// Assert
			using var scope = new AssertionScope();
			result.Result.As<OkObjectResult>().Value.Should().BeOfType<ApiResponse<GetEmployeeDto>>();
			result.Result.As<OkObjectResult>().Value.As<ApiResponse<GetEmployeeDto>>().Data.Id.Should().Be(id);
		}

		[Fact]
		public async Task GivenGetAllIsCalled_ThenReturnsListOfEmployees()
		{
			// Arrange
			var employees = _faker.Generate(5);
			_mediator.Setup(m => m.Send(It.IsAny<GetAllEmployeesQuery>(), default))
				.ReturnsAsync(new ApiResponse<List<GetEmployeeDto>>
				{
					Success = true,
					Data = employees
				});

			// Act
			var result = await _controller.GetAll();

			// Assert
			using var scope = new AssertionScope();
			result.Result.As<OkObjectResult>().Value.Should().BeOfType<ApiResponse<List<GetEmployeeDto>>>();
			result.Result.As<OkObjectResult>().Value.As<ApiResponse<List<GetEmployeeDto>>>().Data.Should().NotBeEmpty();
		}

		[Fact]
		public async Task GivenGetIsCalled_WhenIdIsInvalid_ThenReturnsNotFound()
		{
			// Arrange
			Guid id = Guid.NewGuid();
			_mediator.Setup(m => m.Send(It.IsAny<GetEmployeeByIdQuery>(), default))
				.ReturnsAsync(new ApiResponse<GetEmployeeDto>
				{
					Success = true,
					Data = null
				});

			// Act
			ActionResult<ApiResponse<GetEmployeeDto>> result = await _controller.Get(id);

			// Assert
			result.Result.Should().BeOfType<NotFoundObjectResult>();
		}

		[Fact]
		public async Task GivenGetIsCalled_WhenMediatorReturnsError_ThenReturnsBadRequest()
		{
			// Arrange
			Guid id = Guid.NewGuid();
			_mediator.Setup(m => m.Send(It.IsAny<GetEmployeeByIdQuery>(), default))
				.ReturnsAsync(new ApiResponse<GetEmployeeDto>
				{
					Success = false
				});

			// Act
			ActionResult<ApiResponse<GetEmployeeDto>> result = await _controller.Get(id);

			// Assert
			result.Result.Should().BeOfType<BadRequestObjectResult>();
		}

		[Fact]
		public async Task GivenGetPaycheckIsCalled_WhenIdIsValid_ThenReturnsPaycheck()
		{
			// Arrange
			Guid id = Guid.NewGuid();
			var paycheck = new PaycheckDto();
			_mediator.Setup(m => m.Send(It.IsAny<GetPaycheckQuery>(), default))
				.ReturnsAsync(new ApiResponse<PaycheckDto>
				{
					Success = true,
					Data = paycheck
				});

			// Act
			var result = await _controller.GetPaycheck(id);

			// Assert
			result.Result.As<OkObjectResult>().Value.Should().BeOfType<ApiResponse<PaycheckDto>>();
		}

		[Fact]
		public async Task GivenGetPaycheckIsCalled_WhenIdIsInvalid_ThenReturnsNotFound()
		{
			// Arrange
			Guid id = Guid.NewGuid();
			_mediator.Setup(m => m.Send(It.IsAny<GetPaycheckQuery>(), default))
				.ReturnsAsync(new ApiResponse<PaycheckDto>
				{
					Success = true,
					Data = null
				});

			// Act
			var result = await _controller.GetPaycheck(id);

			// Assert
			result.Result.Should().BeOfType<NotFoundObjectResult>();
		}

		[Fact]
		public async Task GivenGetPaycheckIsCalled_WhenMediatorReturnsError_ThenReturnsBadRequest()
		{
			// Arrange
			Guid id = Guid.NewGuid();
			_mediator.Setup(m => m.Send(It.IsAny<GetPaycheckQuery>(), default))
				.ReturnsAsync(new ApiResponse<PaycheckDto>
				{
					Success = false
				});

			// Act
			var result = await _controller.GetPaycheck(id);

			// Assert
			result.Result.Should().BeOfType<BadRequestObjectResult>();
		}
	}
}
