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
	[property: Required, Compare(nameof(RegisterRequest.Password))] string ConfirmPassword);

public sealed record LoginRequest(
	[property: Required] string UserName,
	[property: Required] string Password);

public sealed record AuthenticationResponse(string AccessToken, DateTime ExpiresAtUtc, string UserName, IReadOnlyCollection<string> Roles);
