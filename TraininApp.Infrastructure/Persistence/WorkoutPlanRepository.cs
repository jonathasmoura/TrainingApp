using TraininApp.Application.Interfaces;
using TraininApp.Domain.Entities;

namespace TraininApp.Infrastructure.Persistence;

public sealed class WorkoutPlanRepository(AcademyDbContext db) : Repository<WorkoutPlan>(db), IWorkoutPlanRepository;

