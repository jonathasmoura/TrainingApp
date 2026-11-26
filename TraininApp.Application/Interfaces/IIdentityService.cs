using TraininApp.Application.Common;

namespace TraininApp.Application.Interfaces;

public interface IIdentityService
{
	Task<Result<AuthenticationResult>> RegisterAsync(RegisterData data, CancellationToken cancellationToken = default);
	Task<Result<AuthenticationResult>> LoginAsync(LoginData data, CancellationToken cancellationToken = default);
	Task<Result> UpdateProfileAsync(Guid userId, ProfileUpdateData data, CancellationToken cancellationToken = default);
	Task<Result> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword, CancellationToken cancellationToken = default);
	Task RequestPasswordResetAsync(string email, CancellationToken cancellationToken = default);
	Task<Result> ResetPasswordAsync(string email, string token, string newPassword, CancellationToken cancellationToken = default);
	Task<Result> LogoutAsync(Guid userId, string? tokenId, DateTime? tokenExpiresAtUtc, CancellationToken cancellationToken = default);
}

public sealed record RegisterData(
	string UserName,
	string FirstName,
	string LastName,
	string Email,
	string MobilePhone,
	string? AlternatePhone,
	string Password,
	string TaxId,
	DateOnly BirthDate,
	string? GuardianName,
	string? GuardianTaxId);

public sealed record LoginData(string UserName, string Password);

public sealed record ProfileUpdateData(
	string FirstName,
	string LastName,
	string MobilePhone,
	string? AlternatePhone,
	DateOnly BirthDate,
	string? GuardianName,
	string? GuardianTaxId);

public sealed record AuthenticationResult(string AccessToken, DateTime ExpiresAtUtc, string UserName, IReadOnlyCollection<string> Roles);
