using TraininApp.Domain.Entities;

namespace TraininApp.Application.Interfaces;

public interface IPlanRepository : IRepository<Plan>
{
	Task<IReadOnlyList<Plan>> ListAsync(CancellationToken cancellationToken = default);
}

