using Api.Cqs.Queries;
using Api.Data.Contracts;
using Api.Dtos.Dependent;
using Api.Models;
using Bogus;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ApiTests.UnitTests.Cqs.Queries
{
	public class GetDependentByIdQueryHandlerTests
	{
		private readonly Faker<GetDependentDto> _faker = new();
		private readonly Mock<ILogger<GetDependentByIdQueryHandler>> _logger = new();
		private readonly Mock<IDependentRepository> _repository = new();
		private readonly GetDependentByIdQueryHandler _handler;

		public GetDependentByIdQueryHandlerTests()
		{
			_handler = new(_logger.Object, _repository.Object);
		}

		[Fact]
		public async Task Handle_WhenDependentNotFound_ReturnsApiResponseWithErrorMessage()
		{
			// Arrange
			var query = new GetDependentByIdQuery(Guid.NewGuid());
			_repository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(default(Dependent));

			// Act
			var result = await _handler.Handle(query, default);

			// Assert
			using var scope = new AssertionScope();
			result.Success.Should().BeTrue();
			result.Data.Should().BeNull();
		}

		[Fact]
		public async Task Handle_WhenDependentFound_ReturnsApiResponseWithDependentDto()
		{
			// Arrange
			var dependent = _faker.Generate();
			var query = new GetDependentByIdQuery(dependent.Id);
			_repository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new Dependent
				{
					Id = dependent.Id,
					FirstName = dependent.FirstName,
					LastName = dependent.LastName,
					Relationship = dependent.Relationship,
					DateOfBirth = dependent.DateOfBirth
				});

			// Act
			var result = await _handler.Handle(query, default);

			// Assert
			using var scope = new AssertionScope();
			result.Success.Should().BeTrue();
			result.Data!.Id.Should().Be(dependent.Id);
			result.Data!.FirstName.Should().Be(dependent.FirstName);
			result.Data!.LastName.Should().Be(dependent.LastName);
			result.Data!.Relationship.Should().Be(dependent.Relationship);
			result.Data!.DateOfBirth.Should().Be(dependent.DateOfBirth);
		}

		[Fact]
		public async Task Handle_WhenExceptionThrown_ReturnsApiResponseWithErrorMessage()
		{
			// Arrange
			var query = new GetDependentByIdQuery(Guid.NewGuid());
			_repository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
				.ThrowsAsync(new Exception("Something bad happened..."));

			// Act
			var result = await _handler.Handle(query, default);

			// Assert
			using var scope = new AssertionScope();
			result.Success.Should().BeFalse();
			result.Error.Should().Be(GetDependentByIdQueryHandler.ERR_MSG_GET_DEPENDENT);
		}

		[Fact]
		public async Task Handle_WhenExceptionThrown_LogsError()
		{
			// Arrange
			var query = new GetDependentByIdQuery(Guid.NewGuid());
			_repository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
				.ThrowsAsync(new Exception("Something bad happened..."));

			// Act
			await _handler.Handle(query, default);

			// Assert
			// Leveraging the NuGet package Moq.ILogger to verify the log messaging is correct rather than
			// writing my own verification logic to keep this simple.
			_logger.VerifyLog(l => l.LogError(It.IsAny<Exception>(), GetDependentByIdQueryHandler.ERR_MSG_GET_DEPENDENT_ID_EX, query.Id));
		}

		[Fact]
		public void Constructor_WhenParameterIsNull_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => new GetDependentByIdQueryHandler(null!, _repository.Object));
			Assert.Throws<ArgumentNullException>(() => new GetDependentByIdQueryHandler(_logger.Object, null!));
		}

	}
}
