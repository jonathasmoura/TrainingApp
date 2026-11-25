using TraininApp.Domain.Entities;

namespace TraininApp.Application.Interfaces;

public interface ITimelineService
{
	Task AddMemberCreatedAsync(Guid memberId, CancellationToken cancellationToken);
	Task AddProfessionalCreatedAsync(Guid professionalId, CancellationToken cancellationToken);
	Task AddEnrollmentCreatedAsync(Guid memberId, Guid enrollmentId, CancellationToken cancellationToken);
	Task AddEnrollmentUpdatedAsync(Guid memberId, Guid enrollmentId, CancellationToken cancellationToken);
	Task AddEnrollmentCancelledAsync(Guid memberId, Guid enrollmentId, CancellationToken cancellationToken);
	Task AddWorkoutPlanCreatedAsync(Guid memberId, Guid professionalId, Guid workoutPlanId, CancellationToken cancellationToken);
	Task AddWorkoutPlanUpdatedAsync(Guid memberId, Guid professionalId, Guid workoutPlanId, CancellationToken cancellationToken);
	Task AddInstallmentPaidAsync(Guid memberId, Guid installmentId, CancellationToken cancellationToken);
	Task<IReadOnlyList<TimelineEntry>> GetMemberTimelineAsync(Guid memberId, DateTime? fromUtc, DateTime? toUtc, string? eventType, int page, int pageSize, CancellationToken cancellationToken);
	Task<IReadOnlyList<TimelineEntry>> GetProfessionalTimelineAsync(Guid professionalId, DateTime? fromUtc, DateTime? toUtc, string? eventType, int page, int pageSize, CancellationToken cancellationToken);
}
