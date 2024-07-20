using Api.Models;
using Api.Rules;
using Bogus;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace ApiTests.UnitTests.Rules;

public class DeductionRuleEngineTests
{
	private readonly Faker<Employee> _faker = new();
	private readonly List<IDeductionRule> _rules = new();
	private DeductionRuleEngine _ruleEngine;

	[Fact]
	public void Constructor_WhenRulesIsNull_ThrowsArgumentNullException()
	{
		// Arrange
		List<IDeductionRule> rules = null!;

		// Act & Assert
		Assert.Throws<ArgumentNullException>(() => new DeductionRuleEngine(rules));
	}

	[Fact]
	public void Calculate_WhenEmployeeIsNull_ThrowsArgumentNullException()
	{
		// Arrange
		Employee employee = null!;
		_ruleEngine = new DeductionRuleEngine(_rules);

		// Act & Assert
		Assert.Throws<ArgumentNullException>(() => _ruleEngine.Calculate(employee));
	}

	[Fact]
	public void Calculate_WhenEmployeeIsNotNull_ReturnsCorrectCost()
	{
		// Arrange
		var employee = _faker.Generate();
		var mockRule = new Mock<IDeductionRule>();
		var mockRule2 = new Mock<IDeductionRule>();
		mockRule.Setup(r => r.Calculate(employee)).Returns(1000);
		mockRule2.Setup(r => r.Calculate(employee)).Returns(2000);
		_rules.Add(mockRule.Object);
		_rules.Add(mockRule2.Object);
		var expectedCost = 3000;
		_ruleEngine = new DeductionRuleEngine(_rules);

		// Act
		var result = _ruleEngine.Calculate(employee);

		// Assert
		result.Should().Be(expectedCost);
	}
}
