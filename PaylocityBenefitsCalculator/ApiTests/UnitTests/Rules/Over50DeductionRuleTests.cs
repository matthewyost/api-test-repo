using Api.Models;
using Api.Rules;
using Bogus;
using FluentAssertions;
using System;
using Xunit;

namespace ApiTests.UnitTests.Rules;

public class Over50DeductionRuleTests
{
	private readonly Over50DeductionRule _rule = new();
	private readonly Faker<Employee> _faker = new();
	private readonly Faker<Dependent> _dependentFaker = new();

	[Fact]
	public void Calculate_WhenEmployeeIsNull_ThrowsArgumentNullException()
	{
		// Arrange
		Employee employee = null!;

		// Act & Assert
		Assert.Throws<ArgumentNullException>(() => _rule.Calculate(employee));
	}

	[Fact]
	public void Calculate_WhenDependentIsUnder50_Returns0()
	{
		// Arrange
		var employee = _faker.Generate();
		employee.Dependents = new[] { _dependentFaker
			.RuleFor(r => r.DateOfBirth, r => r.Date.Past(1, DateTime.UtcNow.AddYears(-49)))
			.Generate() };

		// Act
		var result = _rule.Calculate(employee);

		// Assert
		result.Should().Be(0);
	}

	[Fact]
	public void Calculate_WhenDependentIsOver50_Returns200()
	{
		// Arrange
		var employee = _faker.Generate();
		employee.Dependents = new[] { _dependentFaker
			.RuleFor(r => r.DateOfBirth, r => r.Date.Past(1, DateTime.UtcNow.AddYears(-51)))
			.Generate() };

		// Act
		var result = _rule.Calculate(employee);

		// Assert
		result.Should().Be(200);
	}

	[Fact]
	public void Calculate_WhenEmployeeHasMultipleDependents_ReturnsCorrectCost()
	{
		// Arrange
		var employee = _faker.Generate();
		employee.Dependents = new[]
		{
			_dependentFaker
				.RuleFor(r => r.DateOfBirth, r => r.Date.Past(1, DateTime.UtcNow.AddYears(-51)))
				.Generate(),
			_dependentFaker
				.RuleFor(r => r.DateOfBirth, r => r.Date.Past(1, DateTime.UtcNow.AddYears(-49)))
				.Generate(),
			_dependentFaker
				.RuleFor(r => r.DateOfBirth, r => r.Date.Past(1, DateTime.UtcNow.AddYears(-51)))
				.Generate()
		};

		// Act
		var result = _rule.Calculate(employee);

		// Assert
		result.Should().Be(400);
	}
}
