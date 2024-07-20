namespace Api.Models;

public class Dependent
{
	public Guid Id { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public DateTime DateOfBirth { get; set; }
	public Relationship Relationship { get; set; }
	public Guid EmployeeId { get; set; }
	public Employee? Employee { get; set; }
}
