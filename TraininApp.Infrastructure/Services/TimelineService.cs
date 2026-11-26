using Microsoft.EntityFrameworkCore;
using TraininApp.Application.Interfaces;
using TraininApp.Domain.Entities;
using TraininApp.Infrastructure.Persistence;

namespace TraininApp.Infrastructure.Services;

public sealed class TimelineService(AcademyDbContext dbContext) : ITimelineService
{
	public Task AddMemberCreatedAsync(Guid memberId, CancellationToken cancellationToken) =>
		AddAsync(new TimelineEntry(memberId, null, "MemberCreated", "Membro cadastrado", "O membro foi cadastrado com sucesso.", DateTime.UtcNow, memberId, "Member"), cancellationToken);

	public Task AddProfessionalCreatedAsync(Guid professionalId, CancellationToken cancellationToken) =>
		AddAsync(new TimelineEntry(null, professionalId, "ProfessionalCreated", "Profissional cadastrado", "O profissional foi cadastrado com sucesso.", DateTime.UtcNow, professionalId, "Professional"), cancellationToken);

	public Task AddEnrollmentCreatedAsync(Guid memberId, Guid enrollmentId, CancellationToken cancellationToken) =>
		AddAsync(new TimelineEntry(memberId, null, "EnrollmentCreated", "Matrícula criada", "A matrícula do membro foi criada.", DateTime.UtcNow, enrollmentId, "Enrollment"), cancellationToken);

	public Task AddEnrollmentUpdatedAsync(Guid memberId, Guid enrollmentId, CancellationToken cancellationToken) =>
		AddAsync(new TimelineEntry(memberId, null, "EnrollmentUpdated", "Matrícula atualizada", "Os dados da matrícula foram atualizados.", DateTime.UtcNow, enrollmentId, "Enrollment"), cancellationToken);

	public Task AddEnrollmentCancelledAsync(Guid memberId, Guid enrollmentId, CancellationToken cancellationToken) =>
		AddAsync(new TimelineEntry(memberId, null, "EnrollmentCancelled", "Matrícula cancelada", "A matrícula do membro foi cancelada.", DateTime.UtcNow, enrollmentId, "Enrollment"), cancellationToken);

	public Task AddWorkoutPlanCreatedAsync(Guid memberId, Guid professionalId, Guid workoutPlanId, CancellationToken cancellationToken) =>
		AddAsync(new TimelineEntry(memberId, professionalId, "WorkoutPlanCreated", "Ficha de treino criada", "Uma ficha de treino foi criada para o membro.", DateTime.UtcNow, workoutPlanId, "WorkoutPlan"), cancellationToken);

	public Task AddWorkoutPlanUpdatedAsync(Guid memberId, Guid professionalId, Guid workoutPlanId, CancellationToken cancellationToken) =>
		AddAsync(new TimelineEntry(memberId, professionalId, "WorkoutPlanUpdated", "Ficha de treino atualizada", "A ficha de treino do membro foi atualizada.", DateTime.UtcNow, workoutPlanId, "WorkoutPlan"), cancellationToken);

	public Task AddInstallmentPaidAsync(Guid memberId, Guid installmentId, CancellationToken cancellationToken) =>
		AddAsync(new TimelineEntry(memberId, null, "InstallmentPaid", "Parcela paga", "O pagamento da parcela foi registrado.", DateTime.UtcNow, installmentId, "Installment"), cancellationToken);

	private Task AddAsync(TimelineEntry entry, CancellationToken cancellationToken)
	{
		dbContext.Set<TimelineEntry>().Add(entry);
		return Task.CompletedTask;
	}

	public async Task<IReadOnlyList<TimelineEntry>> GetMemberTimelineAsync(Guid memberId, DateTime? fromUtc, DateTime? toUtc, string? eventType, int page, int pageSize, CancellationToken cancellationToken)
	{
		var query = Filter(dbContext.Set<TimelineEntry>().AsNoTracking(), fromUtc, toUtc, eventType)
			.Where(item => item.MemberId == memberId);
		return await query.OrderByDescending(item => item.OccurredAtUtc).ThenByDescending(item => item.Id)
			.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
	}

	public async Task<IReadOnlyList<TimelineEntry>> GetProfessionalTimelineAsync(Guid professionalId, DateTime? fromUtc, DateTime? toUtc, string? eventType, int page, int pageSize, CancellationToken cancellationToken)
	{
		var query = Filter(dbContext.Set<TimelineEntry>().AsNoTracking(), fromUtc, toUtc, eventType)
			.Where(item => item.ProfessionalId == professionalId);
		return await query.OrderByDescending(item => item.OccurredAtUtc).ThenByDescending(item => item.Id)
			.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
	}

	private static IQueryable<TimelineEntry> Filter(IQueryable<TimelineEntry> query, DateTime? fromUtc, DateTime? toUtc, string? eventType)
	{
		if (fromUtc.HasValue) query = query.Where(item => item.OccurredAtUtc >= fromUtc.Value);
		if (toUtc.HasValue) query = query.Where(item => item.OccurredAtUtc <= toUtc.Value);
		if (!string.IsNullOrWhiteSpace(eventType)) query = query.Where(item => item.EventType == eventType);
		return query;
	}
}
