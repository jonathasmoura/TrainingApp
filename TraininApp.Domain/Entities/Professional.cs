using TraininApp.Domain.Common;
using TraininApp.Domain.Enums;

namespace TraininApp.Domain.Entities;

public sealed class Professional : BaseEntity
{
	private Professional() { }
	public Professional(Guid userId, string name, string taxId, string licenseNumber, DateOnly licenseExpirationDate) { UserId = userId; Name = name; TaxId = taxId; LicenseNumber = licenseNumber; LicenseExpirationDate = licenseExpirationDate; Status = EProfessionalStatus.Active; }
	public Guid UserId { get; private set; }
	public string Name { get; private set; } = string.Empty;
	public string TaxId { get; private set; } = string.Empty;
	public string LicenseNumber { get; private set; } = string.Empty;
	public DateOnly LicenseExpirationDate { get; private set; }
	public EProfessionalStatus Status { get; private set; }
	public void Update(string name, DateOnly licenseExpirationDate, EProfessionalStatus status) { Name = name; LicenseExpirationDate = licenseExpirationDate; Status = status; UpdatedAt = DateTime.UtcNow; }
}

