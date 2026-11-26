using TraininApp.Application.Abstractions;
using TraininApp.Application.Common;

namespace TraininApp.Application.Features.Enrollments;

public sealed record CreateEnrollmentCommand(Guid MemberId, Guid PlanId, DateOnly StartDate) : ICommand<Result<Guid>>;

