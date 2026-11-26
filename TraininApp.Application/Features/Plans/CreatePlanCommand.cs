using TraininApp.Application.Abstractions;
using TraininApp.Application.Common;
using TraininApp.Application.Dtos;

namespace TraininApp.Application.Features.Plans;

public sealed record CreatePlanCommand(string Name, decimal MonthlyBaseAmount, decimal EnrollmentFee, int DurationMonths, int DaysPerWeek) : ICommand<Result<PlanDto>>;

