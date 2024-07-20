using Api.Models;
using Api.Rules;
using Bogus;
using FluentAssertions;
using System;
using Xunit;

namespace ApiTests.UnitTests.Rules;

public class DependentDeductionRuleTests
{
	private readonly Faker<Dependent> _faker = new();
	private readonly DependentDeductionRule _rule = new();

	[Fact]
	public void Calculate_WhenEmployeeIsNull_ThrowsArgumentNullException()
	{
		// Arrange
		Employee employee = null!;

		// Act & Assert
		Assert.Throws<ArgumentNullException>(() => _rule.Calculate(employee));
	}

	[Fact]
	public void Calculate_WhenEmployeeHasNoDependents_Returns0()
	{
		// Arrange
		var employee = new Employee();

		// Act
		var result = _rule.Calculate(employee);

		// Assert
		result.Should().Be(0);
	}

	[Fact]
	public void Calculate_WhenEmployeeHasOneDependent_Returns600()
	{
		// Arrange
		var employee = new Employee
		{
			Dependents = new[] { _faker.Generate() }
		};

		// Act
		var result = _rule.Calculate(employee);

		// Assert
		result.Should().Be(600);
	}

	[Theory]
	[InlineData(2, 1200)]
	[InlineData(3, 1800)]
	[InlineData(4, 2400)]
	[InlineData(5, 3000)]
	public void Calculate_WhenEmployeeHasDependents_ReturnsCost(int numberOfDependents, decimal expectedCost)
	{
		// Arrange
		var employee = new Employee
		{
			Dependents = _faker.Generate(numberOfDependents)
		};

		// Act
		var result = _rule.Calculate(employee);

		// Assert
		result.Should().Be(expectedCost);
	}


}
