using Xunit;

namespace ApiTests.Utilities
{
	/// <summary>
	/// Collection fixture that will host the web application factory that
	/// can be reused between any test classs that are part of the collection.
	/// </summary>
	[CollectionDefinition(nameof(ApiCollectionFixture))]
	public class ApiCollectionFixture : ICollectionFixture<ApiWebApplicationFactory>
	{
	}
}
