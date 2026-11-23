using TraininApp.Domain.Common;

namespace TraininApp.Domain.Entities;

public sealed class WorkoutPlan : BaseEntity
{
	private WorkoutPlan() { }
	public WorkoutPlan(Guid memberId, Guid professionalId, string goal, DateOnly startDate, DateOnly expirationDate) { MemberId = memberId; ProfessionalId = professionalId; Goal = goal; StartDate = startDate; ExpirationDate = expirationDate; }
	public Guid MemberId { get; private set; }
	public Guid ProfessionalId { get; private set; }
	public string Goal { get; private set; } = string.Empty;
	public DateOnly StartDate { get; private set; }
	public DateOnly ExpirationDate { get; private set; }
	public List<WorkoutItem> Items { get; private set; } = [];
}

