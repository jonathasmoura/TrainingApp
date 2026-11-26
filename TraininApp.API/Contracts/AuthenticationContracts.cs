using System.ComponentModel.DataAnnotations;

namespace TraininApp.API.Contracts;

public sealed record RegisterRequest(
	[property: Required, StringLength(50)] string UserName,
	[property: Required, StringLength(100)] string FirstName,
	[property: Required, StringLength(100)] string LastName,
	[property: Required, EmailAddress, StringLength(256)] string Email,
	[property: Required, Phone, StringLength(30)] string MobilePhone,
	[StringLength(30)] string? AlternatePhone,
	[property: Required, MinLength(8), StringLength(100)] string Password,
	[property: Required, Compare(nameof(RegisterRequest.Password))] string ConfirmPassword,
	[property: Required, StringLength(30)] string TaxId,
	[property: Required] DateOnly BirthDate,
	[StringLength(100)] string? GuardianName,
	[StringLength(30)] string? GuardianTaxId);

public sealed record LoginRequest(
	[property: Required] string UserName,
	[property: Required] string Password);

public sealed record UpdateProfileRequest(
	[property: Required, StringLength(100)] string FirstName,
	[property: Required, StringLength(100)] string LastName,
	[property: Required, Phone, StringLength(30)] string MobilePhone,
	[StringLength(30)] string? AlternatePhone,
	[property: Required] DateOnly BirthDate,
	[StringLength(100)] string? GuardianName,
	[StringLength(30)] string? GuardianTaxId);

public sealed record ChangePasswordRequest(
	[property: Required] string CurrentPassword,
	[property: Required, MinLength(8), StringLength(100)] string NewPassword,
	[property: Required, Compare("NewPassword")] string ConfirmPassword);

public sealed record ForgotPasswordRequest([property: Required, EmailAddress] string Email);

public sealed record ResetPasswordRequest(
	[property: Required, EmailAddress] string Email,
	[property: Required] string Token,
	[property: Required, MinLength(8), StringLength(100)] string NewPassword,
	[property: Required, Compare("NewPassword")] string ConfirmPassword);

public sealed record AuthenticationResponse(string AccessToken, DateTime ExpiresAtUtc, string UserName, IReadOnlyCollection<string> Roles);
