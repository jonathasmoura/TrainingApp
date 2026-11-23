using TraininApp.Domain.Entities;

namespace TraininApp.Application.Interfaces;

public interface IMemberRepository : IRepository<Member>
{
	Task<IReadOnlyList<Member>> ListAsync(CancellationToken cancellationToken = default);
}

