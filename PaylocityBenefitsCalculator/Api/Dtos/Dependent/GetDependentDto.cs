using Api.Models;

namespace Api.Dtos.Dependent;

public class GetDependentDto
{
	/// <summary>
	/// Identifier of the dependent inside the application
	/// </summary>
	public Guid Id { get; set; }

	/// <summary>
	/// First Name of the dependent
	/// </summary>
	public string? FirstName { get; set; }

	/// <summary>
	/// Last Name of the dependent
	/// </summary>
	public string? LastName { get; set; }

	/// <summary>
	/// Date of Birth of the dependent
	/// </summary>
	public DateTime DateOfBirth { get; set; }

	/// <summary>
	/// Relationship of the dependent to the employee
	/// </summary>
	public Relationship Relationship { get; set; }
}
