using Microsoft.EntityFrameworkCore;
using TraininApp.Application.Interfaces;
using TraininApp.Domain.Entities;

namespace TraininApp.Infrastructure.Persistence;

public sealed class PlanRepository(AcademyDbContext db) : Repository<Plan>(db), IPlanRepository
{
	public async Task<IReadOnlyList<Plan>> ListAsync(CancellationToken cancellationToken = default) => await db.Plans.Where(x => x.Active).AsNoTracking().ToListAsync(cancellationToken);
}

