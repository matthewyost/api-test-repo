using Api.Dtos.Dependent;

namespace Api.Dtos.Employee;

/// <summary>
/// Employee Data Transfer Object
/// </summary>
public class GetEmployeeDto
{
	/// <summary>
	/// Identifier of the employee inside the application
	/// </summary>
	public Guid Id { get; set; }

	/// <summary>
	/// First Name of the employee
	/// </summary>
	public string? FirstName { get; set; }

	/// <summary>
	/// Last Name of the employee
	/// </summary>
	public string? LastName { get; set; }

	/// <summary>
	/// Salary of the employee
	/// </summary>
	public decimal Salary { get; set; }

	/// <summary>
	/// Date of Birth of the employee
	/// </summary>
	public DateTime DateOfBirth { get; set; }

	/// <summary>
	/// Dependents of the employee
	/// </summary>
	public ICollection<GetDependentDto> Dependents { get; set; } = new List<GetDependentDto>();
}
