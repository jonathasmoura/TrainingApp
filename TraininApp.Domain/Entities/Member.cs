using TraininApp.Domain.Common;

namespace TraininApp.Domain.Entities;

public sealed class Member : BaseEntity
{
	private Member() { }
	public Member(Guid userId, string name, string taxId, DateOnly birthDate, string? guardianName = null, string? guardianTaxId = null) { UserId = userId; Name = name; TaxId = taxId; BirthDate = birthDate; GuardianName = guardianName; GuardianTaxId = guardianTaxId; }
	public Guid UserId { get; private set; }
	public string Name { get; private set; } = string.Empty;
	public string TaxId { get; private set; } = string.Empty;
	public DateOnly BirthDate { get; private set; }
	public string? GuardianName { get; private set; }
	public string? GuardianTaxId { get; private set; }
	public bool ParQSigned { get; private set; }
	public void Update(string name, DateOnly birthDate, string? guardianName, string? guardianTaxId, bool parQ) { Name = name; BirthDate = birthDate; GuardianName = guardianName; GuardianTaxId = guardianTaxId; ParQSigned = parQ; UpdatedAt = DateTime.UtcNow; }
	public void SignParQ() => ParQSigned = true;
}

