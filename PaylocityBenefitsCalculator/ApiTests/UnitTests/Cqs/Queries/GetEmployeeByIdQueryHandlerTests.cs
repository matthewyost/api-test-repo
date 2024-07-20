using Api.Cqs.Queries;
using Api.Data.Contracts;
using Api.Dtos.Employee;
using Api.Models;
using Bogus;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ApiTests.UnitTests.Cqs.Queries
{
	public class GetEmployeeByIdQueryHandlerTests
	{
		private readonly Faker<GetEmployeeDto> _faker = new();
		private readonly Mock<ILogger<GetEmployeeByIdQueryHandler>> _logger = new();
		private readonly Mock<IEmployeeRepository> _repository = new();
		private readonly GetEmployeeByIdQueryHandler _handler;

		public GetEmployeeByIdQueryHandlerTests()
		{
			_handler = new(_logger.Object, _repository.Object);
		}

		[Fact]
		public void Constructor_WhenLoggerIsNull_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => new GetEmployeeByIdQueryHandler(null!, _repository.Object));
			Assert.Throws<ArgumentNullException>(() => new GetEmployeeByIdQueryHandler(_logger.Object, null!));
		}

		[Fact]
		public async Task Handle_WhenEmployeeNotFound_ReturnsApiResponseWithErrorMessage()
		{
			// Arrange
			var query = new GetEmployeeByIdQuery(Guid.NewGuid());
			_repository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(default(Employee));

			// Act
			var result = await _handler.Handle(query, CancellationToken.None);

			// Assert
			using var scope = new AssertionScope();
			result.Success.Should().BeTrue();
			result.Data.Should().BeNull();
		}

		[Fact]
		public async Task Handle_WhenEmployeeFound_ReturnsApiResponseWithEmployeeDto()
		{
			// Arrange
			var employee = _faker.Generate();
			var query = new GetEmployeeByIdQuery(employee.Id);
			_repository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new Employee
				{
					Id = employee.Id,
					FirstName = employee.FirstName,
					LastName = employee.LastName,
					Salary = employee.Salary,
					DateOfBirth = employee.DateOfBirth,
					Dependents = employee.Dependents.Select(d => new Dependent
					{
						Id = d.Id,
						FirstName = d.FirstName,
						LastName = d.LastName,
						DateOfBirth = d.DateOfBirth,
						Relationship = d.Relationship
					}).ToList()
				});

			// Act
			var result = await _handler.Handle(query, CancellationToken.None);

			// Assert
			using var scope = new AssertionScope();
			result.Success.Should().BeTrue();
			result.Data.Should().BeEquivalentTo(employee);
		}

		[Fact]
		public async Task Handle_WhenExceptionThrown_ReturnsApiResponseWithErrorMessage()
		{
			// Arrange
			var query = new GetEmployeeByIdQuery(Guid.NewGuid());
			_repository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
				.ThrowsAsync(new Exception("Something bad happened..."));

			// Act
			var result = await _handler.Handle(query, CancellationToken.None);

			// Assert
			result.Success.Should().BeFalse();
			result.Error.Should().Be(GetEmployeeByIdQueryHandler.ERR_MSG_GET_EMPLOYEE);
		}
	}
}
