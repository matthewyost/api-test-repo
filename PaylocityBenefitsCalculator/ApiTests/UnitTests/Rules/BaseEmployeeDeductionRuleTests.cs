using Api.Models;
using Api.Rules;
using FluentAssertions;
using System;
using Xunit;

namespace ApiTests.UnitTests.Rules;

public class BaseEmployeeDeductionRuleTests
{
	private readonly BaseEmployeeDeductionRule _rule = new();

	[Fact]
	public void Calculate_WhenEmployeeIsNull_ThrowsArgumentNullException()
	{
		// Arrange
		Employee employee = null!;

		// Act & Assert
		Assert.Throws<ArgumentNullException>(() => _rule.Calculate(employee));
	}

	[Fact]
	public void Calculate_WhenEmployeeIsNotNull_Returns1000()
	{
		// Arrange
		var employee = new Employee();

		// Act
		var result = _rule.Calculate(employee);

		// Assert
		result.Should().Be(1000);
	}
}
