using Microsoft.EntityFrameworkCore;
using TraininApp.Application.Interfaces;
using TraininApp.Domain.Entities;

namespace TraininApp.Infrastructure.Persistence;

public sealed class MemberRepository(AcademyDbContext db) : Repository<Member>(db), IMemberRepository
{
	public async Task<IReadOnlyList<Member>> ListAsync(CancellationToken cancellationToken = default) => await db.Members.Where(x => x.Active).AsNoTracking().ToListAsync(cancellationToken);
}

