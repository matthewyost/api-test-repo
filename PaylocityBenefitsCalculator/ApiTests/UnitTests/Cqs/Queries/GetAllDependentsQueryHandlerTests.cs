using Api.Cqs.Queries;
using Api.Data.Contracts;
using Api.Models;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ApiTests.UnitTests.Cqs.Queries
{
	public class GetAllDependentsQueryHandlerTests
	{
		private readonly Mock<ILogger<GetAllDependentsQueryHandler>> _logger = new();
		private readonly Mock<IDependentRepository> _repository = new();
		private readonly GetAllDependentsQueryHandler _handler;

		public GetAllDependentsQueryHandlerTests()
		{
			_handler = new(_logger.Object, _repository.Object);
		}

		[Fact]
		public async Task GivenGetAllDependentsQueryIsCalled_WhenDependentsExist_ThenReturnsDependents()
		{
			// Arrange
			var dependents = new List<Dependent>
			{
				new Dependent
				{
					Id = Guid.NewGuid(),
					FirstName = "John",
					LastName = "Doe",
					DateOfBirth = new DateTime(2000, 1, 1),
					Relationship = Relationship.Child
				}
			};

			_repository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
				.ReturnsAsync(dependents.AsQueryable());

			// Act
			var result = await _handler.Handle(new GetAllDependentsQuery(), CancellationToken.None);

			// Assert
			using var scope = new AssertionScope();
			result.Success.Should().BeTrue();
			result.Data.Should().NotBeEmpty();
			result.Data.Should().HaveCount(dependents.Count);
			result.Data!.First().Id.Should().Be(dependents.First().Id);
			result.Data!.First().FirstName.Should().Be(dependents.First().FirstName);
			result.Data!.First().LastName.Should().Be(dependents.First().LastName);
			result.Data!.First().DateOfBirth.Should().Be(dependents.First().DateOfBirth);
			result.Data!.First().Relationship.Should().Be(dependents.First().Relationship);
		}

		[Fact]
		public async Task GivenGetAllDependentsQueryIsCalled_WhenDependentsDoNotExist_ThenReturnsEmptyList()
		{
			// Arrange
			_repository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
				.ReturnsAsync(Enumerable.Empty<Dependent>().AsQueryable());

			// Act
			var result = await _handler.Handle(new GetAllDependentsQuery(), CancellationToken.None);

			// Assert
			using var scope = new AssertionScope();
			result.Success.Should().BeTrue();
			result.Data.Should().BeEmpty();
		}

		[Fact]
		public async Task GivenGetAllDependentsQueryIsCalled_WhenExceptionOccurs_ThenReturnsFailure()
		{
			// Arrange
			_repository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
				.ThrowsAsync(new Exception());

			// Act
			var result = await _handler.Handle(new GetAllDependentsQuery(), CancellationToken.None);

			// Assert
			using var scope = new AssertionScope();
			result.Success.Should().BeFalse();
			result.Error.Should().Be(GetAllDependentsQueryHandler.ERR_MSG_GET_DEPENDENTS);
		}
	}
}
