using Microsoft.EntityFrameworkCore;
using TraininApp.Application.Interfaces;
using TraininApp.Domain.Entities;
using TraininApp.Domain.Enums;

namespace TraininApp.Infrastructure.Persistence;

public sealed class InstallmentRepository(AcademyDbContext db) : Repository<Installment>(db), IInstallmentRepository
{
	public Task<Installment?> GetWithEnrollmentAsync(Guid id, CancellationToken cancellationToken = default) => db.Installments.Include(x => x.Enrollment).SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
	public async Task<IReadOnlyList<Installment>> PendingAsync(CancellationToken cancellationToken = default) => await db.Installments.Where(x => x.Status == EInstallmentStatus.Pending).AsNoTracking().ToListAsync(cancellationToken);
}

