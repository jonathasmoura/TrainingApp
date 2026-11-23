using TraininApp.Domain.Common;
using TraininApp.Domain.Enums;

namespace TraininApp.Domain.Entities;

public sealed class Enrollment : BaseEntity
{
	private Enrollment() { }
	public Enrollment(Guid memberId, Guid planId, DateOnly startDate, DateOnly endDate, decimal agreedMonthlyAmount) { MemberId = memberId; PlanId = planId; StartDate = startDate; EndDate = endDate; AgreedMonthlyAmount = agreedMonthlyAmount; Status = EEnrollmentStatus.Active; }
	public Guid MemberId { get; private set; }
	public Guid PlanId { get; private set; }
	public DateOnly StartDate { get; private set; }
	public DateOnly EndDate { get; private set; }
	public decimal AgreedMonthlyAmount { get; private set; }
	public EEnrollmentStatus Status { get; private set; }
	public byte[] RowVersion { get; private set; } = [];
	public List<Installment> Installments { get; private set; } = [];
	public void Close() { Status = EEnrollmentStatus.Closed; UpdatedAt = DateTime.UtcNow; }
	public void Cancel() { Status = EEnrollmentStatus.Canceled; UpdatedAt = DateTime.UtcNow; }
}

