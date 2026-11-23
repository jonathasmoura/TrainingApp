using TraininApp.Application.Abstractions;
using TraininApp.Application.Common;
using TraininApp.Application.Interfaces;
using TraininApp.Domain.Services;

namespace TraininApp.Application.Features.Enrollments;

public sealed class CreateEnrollmentHandler(IMemberRepository members, IPlanRepository plans, IEnrollmentRepository enrollments, IUnitOfWork unitOfWork, IEnrollmentDomainService service) : ICommandHandler<CreateEnrollmentCommand, Result<Guid>>
{
	public async Task<Result<Guid>> Handle(CreateEnrollmentCommand command, CancellationToken cancellationToken)
	{
		var member = await members.GetByIdAsync(command.MemberId, cancellationToken);
		var plan = await plans.GetByIdAsync(command.PlanId, cancellationToken);
		if (member is null || plan is null) return Result<Guid>.Failure("not_found", "Member or plan not found.");
		var enrollment = service.GenerateNewEnrollment(member, plan, command.StartDate);
		enrollments.Add(enrollment);
		await unitOfWork.SaveChangesAsync(cancellationToken);
		return Result<Guid>.Success(enrollment.Id);
	}
}

