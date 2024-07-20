using Api.Models;
using Api.Rules;
using Bogus;
using FluentAssertions;
using System;
using Xunit;

namespace ApiTests.UnitTests.Rules;

public class SalaryRangeDeductionRuleTests
{
	private readonly SalaryRangeDeductionRule _rule = new();
	private readonly Faker<Employee> _faker = new();

	[Fact]
	public void Calculate_WhenEmployeeIsNull_ThrowsArgumentNullException()
	{
		// Arrange
		Employee employee = null!;

		// Act & Assert
		Assert.Throws<ArgumentNullException>(() => _rule.Calculate(employee));
	}

	[Theory]
	[InlineData(0, 0)]
	[InlineData(79999, 0)]
	[InlineData(80000, 0)]
	[InlineData(80001, 61.53923076923077)]
	public void Calculate_WhenEmployeeSalaryIsLessThanRange_Returns0(decimal salary, decimal expectedCost)
	{
		// Arrange
		Employee employee = _faker
			.RuleFor(r => r.Salary, salary)
			.Generate();

		// Act
		decimal result = _rule.Calculate(employee);

		// Assert
		result.Should().BeApproximately(expectedCost, 14); // Using the "BeApproximately" method to account for floating point precision
	}
}
