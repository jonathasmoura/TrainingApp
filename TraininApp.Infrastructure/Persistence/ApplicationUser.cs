using Microsoft.AspNetCore.Identity;

namespace TraininApp.Infrastructure.Persistence;

public sealed class ApplicationUser : IdentityUser<Guid>
{
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string? AlternatePhone { get; set; }
	public Guid? MemberId { get; set; }
	public Guid? ProfessionalId { get; set; }
}

