using TraininApp.Domain.Common;
using TraininApp.Domain.Entities;
using TraininApp.Domain.Enums;

namespace TraininApp.Domain.Services;

public sealed class WorkoutPlanDomainService : IWorkoutPlanDomainService
{
	public void ValidatePrescription(Member member, Professional professional, DateOnly startDate)
	{
		if (!member.ParQSigned) throw new DomainException("The member must sign the PAR-Q.");
		if (professional.Status != EProfessionalStatus.Active || professional.LicenseExpirationDate < startDate) throw new DomainException("The professional does not have a valid license.");
	}
}

