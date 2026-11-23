using Microsoft.EntityFrameworkCore;
using TraininApp.Application.Interfaces;
using TraininApp.Domain.Entities;

namespace TraininApp.Infrastructure.Persistence;

public sealed class ExerciseRepository(AcademyDbContext db) : Repository<Exercise>(db), IExerciseRepository
{
	public async Task<IReadOnlyList<Exercise>> ListAsync(CancellationToken cancellationToken = default) => await db.Exercises.Where(x => x.Active).AsNoTracking().ToListAsync(cancellationToken);
}

