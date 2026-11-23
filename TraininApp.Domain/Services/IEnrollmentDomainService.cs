using TraininApp.Domain.Entities;
using TraininApp.Domain.Enums;

namespace TraininApp.Domain.Services;

public interface IEnrollmentDomainService
{
	Enrollment GenerateNewEnrollment(Member member, Plan plan, DateOnly startDate);
	Enrollment SwitchPlan(Enrollment currentEnrollment, Plan newPlan, DateOnly startDate);
	void CancelEnrollment(Enrollment enrollment);
	void SettleInstallment(Installment installment, EPaymentMethod paymentMethod);
}

