using Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Data.Configurations
{
	public class DependentEntityConfiguration : IEntityTypeConfiguration<Dependent>
	{
		public void Configure(EntityTypeBuilder<Dependent> builder)
		{
			builder.HasKey(e => e.Id);
			builder.Property(e => e.Id)
				.HasColumnType("uniqueidentifier")
				.ValueGeneratedOnAdd();

			builder.Property(e => e.FirstName)
				.IsRequired()
				.HasColumnType("nvarchar(50)")
				.HasMaxLength(50); // While not asked for, I am putting this in here to assist with defining the schema.

			builder.Property(e => e.LastName)
				.IsRequired()
				.HasColumnType("nvarchar(50)")
				.HasMaxLength(50); // While not asked for, I am putting this in here to assist with defining the schema.

			builder.Property(e => e.DateOfBirth)
				.IsRequired();

			builder.Property(e => e.Relationship)
				.IsRequired()
				.HasColumnType("int");

			builder.HasOne(e => e.Employee)
				.WithMany(e => e.Dependents)
				.HasForeignKey(e => e.EmployeeId);

			builder.HasData(new[] {
				new Dependent()
				{
					Id = Guid.Parse("a581f888-1c95-4cef-9211-9ce2f8990fd3"),
					FirstName = "Spouse",
					LastName = "Morant",
					Relationship = Relationship.Spouse,
					DateOfBirth = new DateTime(1998, 3, 3),
					EmployeeId = Guid.Parse("212dbc68-6543-4eae-b9e2-6854317a8cda")
				},
				new()
				{
					Id = Guid.Parse("729667e2-ba17-41ac-b98a-4a9f02da451e"),
					FirstName = "Child1",
					LastName = "Morant",
					Relationship = Relationship.Child,
					DateOfBirth = new DateTime(2020, 6, 23),
					EmployeeId = Guid.Parse("212dbc68-6543-4eae-b9e2-6854317a8cda")
				},
				new()
				{
					Id = Guid.Parse("452b5267-6507-4d09-89ed-e2a3eff63d26"),
					FirstName = "Child2",
					LastName = "Morant",
					Relationship = Relationship.Child,
					DateOfBirth = new DateTime(2021, 5, 18),
					EmployeeId = Guid.Parse("212dbc68-6543-4eae-b9e2-6854317a8cda")
				},
				new()
				{
					Id = Guid.Parse("1fe23d8a-1b9e-46b4-a1ef-b60db99eac02"),
					FirstName = "DP",
					LastName = "Jordan",
					Relationship = Relationship.DomesticPartner,
					DateOfBirth = new DateTime(1974, 1, 2),
					EmployeeId = Guid.Parse("6221d62d-fa7f-4b2d-84a9-6216b99c6edf")
				}
			});
		}
	}
}
