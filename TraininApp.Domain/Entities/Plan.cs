using TraininApp.Domain.Common;

namespace TraininApp.Domain.Entities;

public sealed class Plan : BaseEntity
{
	private Plan() { }
	public Plan(string name, decimal monthlyBaseAmount, decimal enrollmentFee, int durationMonths, int daysPerWeek) { Name = name; MonthlyBaseAmount = monthlyBaseAmount; EnrollmentFee = enrollmentFee; DurationMonths = durationMonths; DaysPerWeek = daysPerWeek; }
	public string Name { get; private set; } = string.Empty;
	public decimal MonthlyBaseAmount { get; private set; }
	public decimal EnrollmentFee { get; private set; }
	public int DurationMonths { get; private set; }
	public int DaysPerWeek { get; private set; }
	public void Update(string name, decimal monthlyBaseAmount, decimal enrollmentFee, int durationMonths, int daysPerWeek) { Name = name; MonthlyBaseAmount = monthlyBaseAmount; EnrollmentFee = enrollmentFee; DurationMonths = durationMonths; DaysPerWeek = daysPerWeek; UpdatedAt = DateTime.UtcNow; }
}

