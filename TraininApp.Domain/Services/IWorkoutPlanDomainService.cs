using TraininApp.Domain.Entities;

namespace TraininApp.Domain.Services;

public interface IWorkoutPlanDomainService
{
	void ValidatePrescription(Member member, Professional professional, DateOnly startDate);
}

