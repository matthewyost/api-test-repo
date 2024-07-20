using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using ApiTests.Utilities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace ApiTests.IntegrationTests;

[CollectionDefinition(nameof(ApiCollectionFixture))]
public class EmployeeIntegrationTests : IClassFixture<ApiWebApplicationFactory>
{
	private readonly ApiWebApplicationFactory _factory;

	public EmployeeIntegrationTests(ApiWebApplicationFactory factory)
	{
		_factory = factory;
	}

	[Fact]
	public async Task WhenAskedForAllEmployees_ShouldReturnAllEmployees()
	{
		using var httpClient = _factory.CreateClient();
		httpClient.DefaultRequestHeaders.Add("accept", "text/plain");
		var response = await httpClient.GetAsync("/api/v1/employees");

		var employees = new List<GetEmployeeDto>
		{
			new()
			{
				Id = Guid.Parse("209500a4-bd3f-4787-b139-41ead5138f1f"),
				FirstName = "LeBron",
				LastName = "James",
				Salary = 75420.99m,
				DateOfBirth = new DateTime(1984, 12, 30)
			},
			new()
			{
				Id = Guid.Parse("212dbc68-6543-4eae-b9e2-6854317a8cda"),
				FirstName = "Ja",
				LastName = "Morant",
				Salary = 92365.22m,
				DateOfBirth = new DateTime(1999, 8, 10),
				Dependents = new List<GetDependentDto>
				{
					new()
					{
						Id = Guid.Parse("a581f888-1c95-4cef-9211-9ce2f8990fd3"),
						FirstName = "Spouse",
						LastName = "Morant",
						Relationship = Relationship.Spouse,
						DateOfBirth = new DateTime(1998, 3, 3)
					},
					new()
					{
						Id = Guid.Parse("729667e2-ba17-41ac-b98a-4a9f02da451e"),
						FirstName = "Child1",
						LastName = "Morant",
						Relationship = Relationship.Child,
						DateOfBirth = new DateTime(2020, 6, 23)
					},
					new()
					{
						Id = Guid.Parse("452b5267-6507-4d09-89ed-e2a3eff63d26"),
						FirstName = "Child2",
						LastName = "Morant",
						Relationship = Relationship.Child,
						DateOfBirth = new DateTime(2021, 5, 18)
					}
				}
			},
			new()
			{
				Id = Guid.Parse("6221d62d-fa7f-4b2d-84a9-6216b99c6edf"),
				FirstName = "Michael",
				LastName = "Jordan",
				Salary = 143211.12m,
				DateOfBirth = new DateTime(1963, 2, 17),
				Dependents = new List<GetDependentDto>
				{
					new()
					{
						Id = Guid.Parse("1fe23d8a-1b9e-46b4-a1ef-b60db99eac02"),
						FirstName = "DP",
						LastName = "Jordan",
						Relationship = Relationship.DomesticPartner,
						DateOfBirth = new DateTime(1974, 1, 2)
					}
				}
			}
		};

		response.StatusCode.Should().Be(HttpStatusCode.OK);
		var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<GetEmployeeDto>>>();
		apiResponse.Data.Should().BeEquivalentTo(employees);
	}

	[Fact]
	//task: make test pass
	public async Task WhenAskedForAnEmployee_ShouldReturnCorrectEmployee()
	{
		using var httpClient = _factory.CreateClient();
		httpClient.DefaultRequestHeaders.Add("accept", "text/plain");
		var response = await httpClient.GetAsync("/api/v1/employees/209500a4-bd3f-4787-b139-41ead5138f1f");

		var employee = new GetEmployeeDto
		{
			Id = Guid.Parse("209500a4-bd3f-4787-b139-41ead5138f1f"),
			FirstName = "LeBron",
			LastName = "James",
			Salary = 75420.99m,
			DateOfBirth = new DateTime(1984, 12, 30)
		};
		await response.ShouldReturn(HttpStatusCode.OK, employee);
	}

	[Fact]
	//task: make test pass
	public async Task WhenAskedForANonexistentEmployee_ShouldReturn404()
	{
		using var httpClient = _factory.CreateClient();
		httpClient.DefaultRequestHeaders.Add("accept", "text/plain");
		var response = await httpClient.GetAsync($"/api/v1/employees/{Guid.Parse("d8ac3ea9-4445-4c2e-855a-673d7261f44f")}");

		await response.ShouldReturn(HttpStatusCode.NotFound);
	}
}

