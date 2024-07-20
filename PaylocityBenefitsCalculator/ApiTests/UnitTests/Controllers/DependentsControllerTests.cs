using Api.Controllers;
using Api.Cqs.Queries;
using Api.Dtos.Dependent;
using Api.Models;
using Bogus;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ApiTests.UnitTests.Controllers
{

	public class DependentsControllerTests
	{
		private readonly Faker _faker = new();
		private readonly Mock<IMediator> _mediator = new();
		private readonly DependentsController _controller;

		public DependentsControllerTests()
		{
			_controller = new(_mediator.Object);
		}

		[Fact]
		public async Task GivenGetIsCalled_WhenIdIsValid_ThenReturnsDependent()
		{
			// Arrange
			var id = _faker.Random.Guid();
			GetDependentDto dependent = new Faker<GetDependentDto>()
				.RuleFor(d => d.Id, f => id)
				.Generate();

			_mediator.Setup(m => m.Send(It.IsAny<GetDependentByIdQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new ApiResponse<GetDependentDto>
				{
					Success = true,
					Data = dependent
				});

			// Act
			var result = await _controller.Get(id);

			// Assert
			using var scope = new AssertionScope();
			result.Result.As<OkObjectResult>().Value.Should().BeOfType<ApiResponse<GetDependentDto>>();
			result.Result.As<OkObjectResult>().Value.As<ApiResponse<GetDependentDto>>().Data.Id.Should().Be(id);
		}

		[Fact]
		public async Task GivenGetAllIsCalled_ThenReturnsListOfDependents()
		{
			// Arrange
			var dependents = new Faker<GetDependentDto>()
				.RuleFor(d => d.Id, f => f.Random.Guid())
				.Generate(5);

			_mediator.Setup(m => m.Send(It.IsAny<GetAllDependentsQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new ApiResponse<List<GetDependentDto>>
				{
					Success = true,
					Data = dependents
				});

			// Act
			var result = await _controller.GetAll();

			// Assert
			using var scope = new AssertionScope();
			result.Result.As<OkObjectResult>().Value.Should().BeOfType<ApiResponse<List<GetDependentDto>>>();
			result.Result.As<OkObjectResult>().Value.As<ApiResponse<List<GetDependentDto>>>().Data.Should().NotBeEmpty();
		}
	}
}
