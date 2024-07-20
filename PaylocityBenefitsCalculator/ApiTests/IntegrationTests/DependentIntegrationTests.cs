using Api.Dtos.Dependent;
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
public class DependentIntegrationTests : IClassFixture<ApiWebApplicationFactory>
{
	private readonly ApiWebApplicationFactory _factory;

	public DependentIntegrationTests(ApiWebApplicationFactory factory)
	{
		_factory = factory;
	}

	[Fact]
	public async Task WhenAskedForAllDependents_ShouldReturnAllDependents()
	{
		using var httpClient = _factory.CreateClient();
		httpClient.DefaultRequestHeaders.Add("accept", "text/plain");
		var response = await httpClient.GetAsync("/api/v1/dependents");

		var dependents = new List<GetDependentDto>
		{
			new()
			{
				Id = Guid.Parse("A581F888-1C95-4CEF-9211-9CE2F8990FD3"),
				FirstName = "Spouse",
				LastName = "Morant",
				Relationship = Relationship.Spouse,
				DateOfBirth = new DateTime(1998, 3, 3)
			},
			new()
			{
					Id = Guid.Parse("729667E2-BA17-41AC-B98A-4A9F02DA451E"),
					FirstName = "Child1",
					LastName = "Morant",
					Relationship = Relationship.Child,
					DateOfBirth = new DateTime(2020, 6, 23)
			},
			new()
			{
					Id = Guid.Parse("452B5267-6507-4D09-89ED-E2A3EFF63D26"),
					FirstName = "Child2",
					LastName = "Morant",
					Relationship = Relationship.Child,
					DateOfBirth = new DateTime(2021, 5, 18)
			},
			new()
			{
					Id = Guid.Parse("1FE23D8A-1B9E-46B4-A1EF-B60DB99EAC02"),
					FirstName = "DP",
					LastName = "Jordan",
					Relationship = Relationship.DomesticPartner,
					DateOfBirth = new DateTime(1974, 1, 2)
			}
		};

		response.StatusCode.Should().Be(HttpStatusCode.OK);
		var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<GetDependentDto>>>();
		apiResponse.Data.Should().BeEquivalentTo(dependents);
	}

	[Fact]
	//task: make test pass
	public async Task WhenAskedForADependent_ShouldReturnCorrectDependent()
	{
		using var httpClient = _factory.CreateClient();
		httpClient.DefaultRequestHeaders.Add("accept", "text/plain");
		var response = await httpClient.GetAsync("/api/v1/dependents/A581F888-1C95-4CEF-9211-9CE2F8990FD3");

		var dependent = new GetDependentDto
		{
			Id = Guid.Parse("A581F888-1C95-4CEF-9211-9CE2F8990FD3"),
			FirstName = "Spouse",
			LastName = "Morant",
			Relationship = Relationship.Spouse,
			DateOfBirth = new DateTime(1998, 3, 3)
		};
		await response.ShouldReturn(HttpStatusCode.OK, dependent);
	}

	[Fact]
	//task: make test pass
	public async Task WhenAskedForANonexistentDependent_ShouldReturn404()
	{
		using var httpClient = _factory.CreateClient();
		httpClient.DefaultRequestHeaders.Add("accept", "text/plain");
		var response = await httpClient.GetAsync($"/api/v1/dependents/{Guid.Parse("525e7b84-3720-45bd-8d75-43826ff6f93f")}");

		await response.ShouldReturn(HttpStatusCode.NotFound);
	}
}

