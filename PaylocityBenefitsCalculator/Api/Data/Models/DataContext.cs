using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Models
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions options) : base(options)
		{
		}


		#region Properties

		public DbSet<Dependent> Dependents { get; set; }

		public DbSet<Employee> Employees { get; set; }

		#endregion

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);

			base.OnModelCreating(modelBuilder);
		}
	}
}
