using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
	public partial class InitialCreate : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Employees",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
					LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
					Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
					DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Employees", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Dependents",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
					LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
					DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
					Relationship = table.Column<int>(type: "int", nullable: false),
					EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Dependents", x => x.Id);
					table.ForeignKey(
						name: "FK_Dependents_Employees_EmployeeId",
						column: x => x.EmployeeId,
						principalTable: "Employees",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			// Since SQL Server cannot do a subquery in a check constraint, we need to create a function to do this.
			migrationBuilder.Sql(@"CREATE FUNCTION fnValidateDependentRelationship (@EmployeeId uniqueidentifier) RETURNS INT
AS
BEGIN
	RETURN (
	SELECT COUNT(*)
	FROM [Dependents] d
	WHERE d.EmployeeId = @EmployeeId 
		AND d.Relationship IN (1,2))
END;");

			// Now we can create the check constraint
			migrationBuilder.Sql(@"ALTER TABLE [Dependents] ADD CONSTRAINT CK_Dependents_Relationship CHECK (dbo.fnValidateDependentRelationship([EmployeeId]) <= 1)");

			// Seed the database with some sample data
			migrationBuilder.InsertData(
				table: "Employees",
				columns: new[] { "Id", "DateOfBirth", "FirstName", "LastName", "Salary" },
				values: new object[] { new Guid("209500a4-bd3f-4787-b139-41ead5138f1f"), new DateTime(1984, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "LeBron", "James", 75420.99m });

			migrationBuilder.InsertData(
				table: "Employees",
				columns: new[] { "Id", "DateOfBirth", "FirstName", "LastName", "Salary" },
				values: new object[] { new Guid("212dbc68-6543-4eae-b9e2-6854317a8cda"), new DateTime(1999, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ja", "Morant", 92365.22m });

			migrationBuilder.InsertData(
				table: "Employees",
				columns: new[] { "Id", "DateOfBirth", "FirstName", "LastName", "Salary" },
				values: new object[] { new Guid("6221d62d-fa7f-4b2d-84a9-6216b99c6edf"), new DateTime(1963, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Michael", "Jordan", 143211.12m });

			migrationBuilder.InsertData(
				table: "Dependents",
				columns: new[] { "Id", "DateOfBirth", "EmployeeId", "FirstName", "LastName", "Relationship" },
				values: new object[,]
				{
					{ new Guid("1fe23d8a-1b9e-46b4-a1ef-b60db99eac02"), new DateTime(1974, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("6221d62d-fa7f-4b2d-84a9-6216b99c6edf"), "DP", "Jordan", 2 },
					{ new Guid("452b5267-6507-4d09-89ed-e2a3eff63d26"), new DateTime(2021, 5, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("212dbc68-6543-4eae-b9e2-6854317a8cda"), "Child2", "Morant", 3 },
					{ new Guid("729667e2-ba17-41ac-b98a-4a9f02da451e"), new DateTime(2020, 6, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("212dbc68-6543-4eae-b9e2-6854317a8cda"), "Child1", "Morant", 3 },
					{ new Guid("a581f888-1c95-4cef-9211-9ce2f8990fd3"), new DateTime(1998, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("212dbc68-6543-4eae-b9e2-6854317a8cda"), "Spouse", "Morant", 1 }
				});

			migrationBuilder.CreateIndex(
				name: "IX_Dependents_EmployeeId",
				table: "Dependents",
				column: "EmployeeId");


		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			// During the removal of the tables, we need to remove the check constraint and function
			migrationBuilder.Sql(@"ALTER TABLE [Dependents] DROP CONSTRAINT CK_Dependents_Relationship");
			migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS [dbo].[fnValidateDependentRelationship]");

			migrationBuilder.DropTable(
				name: "Dependents");

			migrationBuilder.DropTable(
				name: "Employees");

		}
	}
}
