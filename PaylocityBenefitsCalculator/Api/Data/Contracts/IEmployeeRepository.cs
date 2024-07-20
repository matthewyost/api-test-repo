using Api.Models;

namespace Api.Data.Contracts
{
	/// <summary>
	/// Repository for employees
	/// </summary>
	public interface IEmployeeRepository
	{
		/// <summary>
		/// Retrieve all employees
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<IQueryable<Employee>> GetAllAsync(CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve a specific employee by id
		/// </summary>
		/// <param name="id"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<Employee> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
	}
}
