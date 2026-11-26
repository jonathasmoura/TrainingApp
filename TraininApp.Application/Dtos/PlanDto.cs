namespace TraininApp.Application.Dtos;

public sealed record PlanDto(Guid Id, string Name, decimal MonthlyBaseAmount, decimal EnrollmentFee, int DurationMonths, int DaysPerWeek);

