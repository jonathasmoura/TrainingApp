using TraininApp.Application.Abstractions;
using TraininApp.Application.Common;
using TraininApp.Application.Dtos;
using TraininApp.Application.Interfaces;
using TraininApp.Domain.Entities;

namespace TraininApp.Application.Features.Plans;

public sealed class CreatePlanHandler(IPlanRepository repository, IUnitOfWork unitOfWork) : ICommandHandler<CreatePlanCommand, Result<PlanDto>>
{
	public async Task<Result<PlanDto>> Handle(CreatePlanCommand command, CancellationToken cancellationToken)
	{
		var plan = new Plan(command.Name, command.MonthlyBaseAmount, command.EnrollmentFee, command.DurationMonths, command.DaysPerWeek);
		repository.Add(plan);
		await unitOfWork.SaveChangesAsync(cancellationToken);
		return Result<PlanDto>.Success(new PlanDto(plan.Id, plan.Name, plan.MonthlyBaseAmount, plan.EnrollmentFee, plan.DurationMonths, plan.DaysPerWeek));
	}
}

