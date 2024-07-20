using Api.Models;

namespace Api.Data.Contracts
{
	/// <summary>
	/// Repository for dependents
	/// </summary>
	public interface IDependentRepository
	{
		/// <summary>
		/// Retrieve all dependents
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<IQueryable<Dependent>> GetAllAsync(CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieve a specific dependent by id
		/// </summary>
		/// <param name="id"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<Dependent> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
	}
}
