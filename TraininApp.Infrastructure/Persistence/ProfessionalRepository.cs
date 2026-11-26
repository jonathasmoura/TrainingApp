using Microsoft.EntityFrameworkCore;
using TraininApp.Application.Interfaces;
using TraininApp.Domain.Entities;

namespace TraininApp.Infrastructure.Persistence;

public sealed class ProfessionalRepository(AcademyDbContext db) : Repository<Professional>(db), IProfessionalRepository
{
	public async Task<IReadOnlyList<Professional>> ListAsync(CancellationToken cancellationToken = default) => await db.Professionals.Where(x => x.Active).AsNoTracking().ToListAsync(cancellationToken);
}

