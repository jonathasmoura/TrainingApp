using TraininApp.Application.Interfaces;

namespace TraininApp.Infrastructure.Persistence;

public sealed class UnitOfWork(AcademyDbContext db) : IUnitOfWork
{
	public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => db.SaveChangesAsync(cancellationToken);
}

