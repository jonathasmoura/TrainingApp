using Microsoft.EntityFrameworkCore;
using TraininApp.Domain.Entities;
using TraininApp.Domain.Enums;

namespace TraininApp.Infrastructure.Persistence;

public sealed class DatabaseSeeder(AcademyDbContext db)
{
	public async Task SeedAsync(CancellationToken cancellationToken = default)
	{
		await db.Database.MigrateAsync(cancellationToken);
		if (!await db.Plans.AnyAsync(cancellationToken))
		{
			db.Plans.AddRange(new Plan("Monthly Plan", 310m, 0m, 1, 5), new Plan("Semiannual Plan", 590m, 0m, 6, 5), new Plan("Annual Plan", 3200m, 0m, 12, 5));
			db.Exercises.AddRange(new Exercise("Bench Press", EMuscleGroup.Chest, "Barbell", EDifficultyLevel.Beginner), new Exercise("Squat", EMuscleGroup.Legs, "Barbell", EDifficultyLevel.Intermediate), new Exercise("Low Row", EMuscleGroup.Back, "Cable", EDifficultyLevel.Beginner));
			await db.SaveChangesAsync(cancellationToken);
		}
	}
}

