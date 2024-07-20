using Api.Data.Contracts;
using Api.Data.Models;
using Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Api.Data;

/// <inheritdoc />
public class DependentRepository : IDependentRepository
{
	private readonly DataContext _dataContext;
	private readonly IMemoryCache _memCache;

	public DependentRepository(DataContext dataContext, IMemoryCache memCache)
	{
		_dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
		_memCache = memCache ?? throw new ArgumentNullException(nameof(memCache));
	}

	/// <inheritdoc />
	public Task<IQueryable<Dependent>> GetAllAsync(CancellationToken cancellationToken = default)
	{
		return Task.FromResult(_dataContext.Dependents
			.Include(e => e.Employee)
			.AsQueryable());
	}

	/// <inheritdoc />
	public Task<Dependent> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
	{
		Dependent dependent;

		// Note:  Typically would not do this in a real-world application.  This is just for demonstration purposes.
		// and to keep the example simple.
		if (!_memCache.TryGetValue(id, out dependent))
		{
			dependent = _dataContext.Dependents.Include(e => e.Employee).FirstOrDefault(e => e.Id == id);
			if (dependent is not null)
				_memCache.CreateEntry(id).SetValue(dependent);
		}

		return Task.FromResult(dependent);
	}
}
