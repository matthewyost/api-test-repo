using Api.Cqs.Queries;
using Api.Data.Contracts;
using Api.Models;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ApiTests.UnitTests.Cqs.Queries
{
	public class GetAllEmployeesQueryHandlerTests
	{
		private readonly Faker<Employee> _faker = new();
		private readonly Mock<ILogger<GetAllEmployeesQueryHandler>> _logger = new();
		private readonly Mock<IEmployeeRepository> _repository = new();
		private readonly GetAllEmployeesQueryHandler _handler;

		public GetAllEmployeesQueryHandlerTests()
		{
			_handler = new(_repository.Object, _logger.Object);
		}

		[Fact]
		public async Task Handle_WhenEmployeesFound_ReturnsApiResponseWithEmployeeDtos()
		{
			// Arrange
			var employees = _faker.Generate(3);
			var query = new GetAllEmployeesQuery();
			_repository.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
				.ReturnsAsync(employees.AsQueryable());

			// Act
			var result = await _handler.Handle(query, default);

			// Assert
			result.Success.Should().BeTrue();
			result.Data.Should().HaveCount(3);
			result.Data.Should().BeEquivalentTo(employees);
		}

		[Fact]
		public async Task Handle_WhenExceptionThrown_ReturnsApiResponseWithErrorMessage()
		{
			// Arrange
			var query = new GetAllEmployeesQuery();
			_repository.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
				.ThrowsAsync(new Exception("Something bad happened..."));

			// Act
			var result = await _handler.Handle(query, default);

			// Assert
			result.Success.Should().BeFalse();
			result.Error.Should().Be(GetAllEmployeesQueryHandler.ERR_MSG_GET_EMPLOYEES);
		}
	}
}
