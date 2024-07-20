using Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Data.Configurations
{
	public class EmployeeEntityConfiguration : IEntityTypeConfiguration<Employee>
	{
		public void Configure(EntityTypeBuilder<Employee> builder)
		{
			builder.HasKey(e => e.Id);
			builder.Property(e => e.Id)
				.HasColumnType("uniqueidentifier")
				.ValueGeneratedOnAdd();

			builder.Property(e => e.FirstName)
				.HasColumnType("nvarchar(50)")
				.IsRequired()
				.HasMaxLength(50);

			builder.Property(e => e.Salary)
				.IsRequired(); ;

			builder.Property(e => e.DateOfBirth)
				.IsRequired();

			builder.Property(e => e.LastName)
				.HasColumnType("nvarchar(50)")
				.IsRequired()
				.HasMaxLength(50);

			builder.HasMany(e => e.Dependents).WithOne(e => e.Employee).HasForeignKey(e => e.EmployeeId);

			// Load up sample data here
			builder.HasData(new[] {
				new Employee
				{
					Id = Guid.Parse("209500a4-bd3f-4787-b139-41ead5138f1f"),
					FirstName = "LeBron",
					LastName = "James",
					Salary = 75420.99m,
					DateOfBirth = new DateTime(1984, 12, 30)
				},
				new Employee {
					Id = Guid.Parse("212dbc68-6543-4eae-b9e2-6854317a8cda"),
					FirstName = "Ja",
					LastName = "Morant",
					Salary = 92365.22m,
					DateOfBirth = new DateTime(1999, 8, 10)
				},
				new()
				{
					Id = Guid.Parse("6221d62d-fa7f-4b2d-84a9-6216b99c6edf"),
					FirstName = "Michael",
					LastName = "Jordan",
					Salary = 143211.12m,
					DateOfBirth = new DateTime(1963, 2, 17)
				}
			});

		}
	}
}
