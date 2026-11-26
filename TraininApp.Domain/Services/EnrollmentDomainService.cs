using TraininApp.Domain.Common;
using TraininApp.Domain.Entities;
using TraininApp.Domain.Enums;

namespace TraininApp.Domain.Services;

public sealed class EnrollmentDomainService : IEnrollmentDomainService
{
	public Enrollment GenerateNewEnrollment(Member member, Plan plan, DateOnly startDate)
	{
		if (!member.Active || !plan.Active) throw new DomainException("Member and plan must be active.");
		var enrollment = new Enrollment(member.Id, plan.Id, startDate, startDate.AddMonths(plan.DurationMonths), plan.MonthlyBaseAmount);
		for (var i = 1; i <= plan.DurationMonths; i++) enrollment.Installments.Add(new Installment(enrollment.Id, i, startDate.AddMonths(i - 1), plan.MonthlyBaseAmount + (i == 1 ? plan.EnrollmentFee : 0)));
		return enrollment;
	}

	public Enrollment SwitchPlan(Enrollment currentEnrollment, Plan newPlan, DateOnly startDate)
	{
		currentEnrollment.Close();
		foreach (var installment in currentEnrollment.Installments.Where(x => x.Status == EInstallmentStatus.Pending)) installment.Cancel();
		return new Enrollment(currentEnrollment.MemberId, newPlan.Id, startDate, startDate.AddMonths(newPlan.DurationMonths), newPlan.MonthlyBaseAmount);
	}

	public void CancelEnrollment(Enrollment enrollment)
	{
		enrollment.Cancel();
		foreach (var installment in enrollment.Installments) installment.Cancel();
	}

	public void SettleInstallment(Installment installment, EPaymentMethod paymentMethod) => installment.Settle(paymentMethod);
}

