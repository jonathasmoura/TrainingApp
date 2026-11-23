using TraininApp.Domain.Entities;

namespace TraininApp.Application.Interfaces;

public interface IProfessionalRepository : IRepository<Professional>
{
	Task<IReadOnlyList<Professional>> ListAsync(CancellationToken cancellationToken = default);
}

