using Api.Data.Contracts;
using Api.Data.Models;
using Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Api.Data;

/// <inheritdoc />
public class EmployeeRepository : IEmployeeRepository
{
	private readonly DataContext _dataContext;
	private readonly IMemoryCache _memCache;

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="dataContext"></param>
	/// <exception cref="ArgumentNullException"></exception>
	public EmployeeRepository(DataContext dataContext, IMemoryCache memCache)
	{
		_dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
		_memCache = memCache ?? throw new ArgumentNullException(nameof(memCache));
	}

	/// <inheritdoc />
	public Task<IQueryable<Employee>> GetAllAsync(CancellationToken cancellationToken = default)
	{
		// Note:  Typically would not do this in a real-world application.  This is just for demonstration purposes.
		// and to keep the example simple.
		return Task.FromResult(_dataContext.Employees.Include(e => e.Dependents).AsQueryable());
	}

	/// <inheritdoc />
	public Task<Employee> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
	{
		// Leveraging caching to improve performance and reduce calls the database
		Employee employee;
		if (!_memCache.TryGetValue(id, out employee))
		{
			employee = _dataContext.Employees.Include(e => e.Dependents).FirstOrDefault(e => e.Id == id);
			if (employee is not null)
				_memCache.CreateEntry(id).SetValue(employee);
		}

		return Task.FromResult(employee);
	}
}
