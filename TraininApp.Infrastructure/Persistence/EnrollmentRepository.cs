using Microsoft.EntityFrameworkCore;
using TraininApp.Application.Interfaces;
using TraininApp.Domain.Entities;

namespace TraininApp.Infrastructure.Persistence;

public sealed class EnrollmentRepository(AcademyDbContext db) : Repository<Enrollment>(db), IEnrollmentRepository
{
	public Task<Enrollment?> GetWithInstallmentsAsync(Guid id, CancellationToken cancellationToken = default) => db.Enrollments.Include(x => x.Installments).SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
}

