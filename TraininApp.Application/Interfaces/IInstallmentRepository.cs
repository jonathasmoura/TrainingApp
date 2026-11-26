using TraininApp.Domain.Entities;

namespace TraininApp.Application.Interfaces;

public interface IInstallmentRepository : IRepository<Installment>
{
	Task<Installment?> GetWithEnrollmentAsync(Guid id, CancellationToken cancellationToken = default);
	Task<IReadOnlyList<Installment>> PendingAsync(CancellationToken cancellationToken = default);
}

