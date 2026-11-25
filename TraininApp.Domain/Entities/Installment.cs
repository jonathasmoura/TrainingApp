using TraininApp.Domain.Common;
using TraininApp.Domain.Enums;

namespace TraininApp.Domain.Entities;

public sealed class Installment : BaseEntity
{
	private Installment() { }
	public Installment(Guid enrollmentId, int installmentNumber, DateOnly dueDate, decimal amount) { EnrollmentId = enrollmentId; InstallmentNumber = installmentNumber; DueDate = dueDate; Amount = amount; Status = EInstallmentStatus.Pending; }
	public Guid EnrollmentId { get; private set; }
	public Enrollment Enrollment { get; private set; } = null!;
	public int InstallmentNumber { get; private set; }
	public DateOnly DueDate { get; private set; }
	public decimal Amount { get; private set; }
	public DateTime? PaymentDate { get; private set; }
	public EPaymentMethod? PaymentMethod { get; private set; }
	public EInstallmentStatus Status { get; private set; }
	public byte[] RowVersion { get; private set; } = [];
	public void Settle(EPaymentMethod paymentMethod) { if (Status is EInstallmentStatus.Paid or EInstallmentStatus.Canceled) throw new DomainException("Installment cannot be settled."); Status = EInstallmentStatus.Paid; PaymentMethod = paymentMethod; PaymentDate = DateTime.UtcNow; }
	public void Cancel() { if (Status == EInstallmentStatus.Pending) Status = EInstallmentStatus.Canceled; }
}

