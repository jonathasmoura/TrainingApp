using TraininApp.Domain.Entities;

namespace TraininApp.Application.Interfaces;

public interface IReadOnlyAcademyContext
{
	IQueryable<Member> Members { get; }
	IQueryable<Professional> Professionals { get; }
	IQueryable<Exercise> Exercises { get; }
	IQueryable<Plan> Plans { get; }
}

