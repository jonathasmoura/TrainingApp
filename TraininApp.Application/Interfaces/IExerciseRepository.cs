using TraininApp.Domain.Entities;

namespace TraininApp.Application.Interfaces;

public interface IExerciseRepository : IRepository<Exercise>
{
	Task<IReadOnlyList<Exercise>> ListAsync(CancellationToken cancellationToken = default);
}

