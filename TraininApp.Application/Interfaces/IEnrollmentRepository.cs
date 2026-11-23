using TraininApp.Domain.Entities;

namespace TraininApp.Application.Interfaces;

public interface IEnrollmentRepository : IRepository<Enrollment>
{
	Task<Enrollment?> GetWithInstallmentsAsync(Guid id, CancellationToken cancellationToken = default);
}

