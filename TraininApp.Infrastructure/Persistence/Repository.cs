using Microsoft.EntityFrameworkCore;
using TraininApp.Application.Interfaces;

namespace TraininApp.Infrastructure.Persistence;

public class Repository<T>(AcademyDbContext db) : IRepository<T> where T : class
{
	public Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => db.Set<T>().FindAsync([id], cancellationToken).AsTask();
	public void Add(T entity) => db.Set<T>().Add(entity);
	public void Update(T entity) => db.Set<T>().Update(entity);
}

