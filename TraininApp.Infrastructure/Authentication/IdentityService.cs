using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TraininApp.Application.Common;
using TraininApp.Application.Interfaces;
using TraininApp.Domain.Entities;
using TraininApp.Infrastructure.Persistence;

namespace TraininApp.Infrastructure.Authentication;

public sealed class IdentityService(
	UserManager<ApplicationUser> userManager,
	SignInManager<ApplicationUser> signInManager,
	RoleManager<IdentityRole<Guid>> roleManager,
	AcademyDbContext db,
	ITokenService tokenService,
	ITokenBlacklistService tokenBlacklist,
	IPasswordResetEmailSender passwordResetEmailSender) : IIdentityService
{
	private const string MemberRole = "Member";

	public async Task<Result<AuthenticationResult>> RegisterAsync(RegisterData data, CancellationToken cancellationToken = default)
	{
		await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);
		var user = new ApplicationUser
		{
			UserName = data.UserName.Trim(),
			Email = data.Email.Trim(),
			PhoneNumber = data.MobilePhone.Trim(),
			FirstName = data.FirstName.Trim(),
			LastName = data.LastName.Trim(),
			AlternatePhone = data.AlternatePhone?.Trim()
		};

		var createResult = await userManager.CreateAsync(user, data.Password);
		if (!createResult.Succeeded) return Failure<AuthenticationResult>(createResult);

		if (!await roleManager.RoleExistsAsync(MemberRole))
		{
			var roleResult = await roleManager.CreateAsync(new IdentityRole<Guid>(MemberRole));
			if (!roleResult.Succeeded) return Failure<AuthenticationResult>(roleResult);
		}

		var member = new Member(user.Id, $"{data.FirstName.Trim()} {data.LastName.Trim()}", data.TaxId.Trim(), data.BirthDate, data.GuardianName?.Trim(), data.GuardianTaxId?.Trim());
		db.Members.Add(member);
		var roleAssignment = await userManager.AddToRoleAsync(user, MemberRole);
		if (!roleAssignment.Succeeded) return Failure<AuthenticationResult>(roleAssignment);

		await db.SaveChangesAsync(cancellationToken);
		await transaction.CommitAsync(cancellationToken);
		return Result<AuthenticationResult>.Success(CreateAuthenticationResult(user, [MemberRole]));
	}

	public async Task<Result<AuthenticationResult>> LoginAsync(LoginData data, CancellationToken cancellationToken = default)
	{
		var user = await userManager.FindByNameAsync(data.UserName.Trim());
		if (user is null || !(await signInManager.CheckPasswordSignInAsync(user, data.Password, lockoutOnFailure: true)).Succeeded)
			return Result<AuthenticationResult>.Failure("invalid_credentials", "Usuário ou senha inválidos.");

		var roles = await userManager.GetRolesAsync(user);
		return Result<AuthenticationResult>.Success(CreateAuthenticationResult(user, roles));
	}

	public async Task<Result> UpdateProfileAsync(Guid userId, ProfileUpdateData data, CancellationToken cancellationToken = default)
	{
		var user = await userManager.FindByIdAsync(userId.ToString());
		var member = await db.Members.SingleOrDefaultAsync(x => x.UserId == userId, cancellationToken);
		if (user is null || member is null) return Result.Failure("not_found", "Perfil não encontrado.");

		await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);
		user.FirstName = data.FirstName.Trim();
		user.LastName = data.LastName.Trim();
		user.PhoneNumber = data.MobilePhone.Trim();
		user.AlternatePhone = data.AlternatePhone?.Trim();
		var identityResult = await userManager.UpdateAsync(user);
		if (!identityResult.Succeeded) return Failure(identityResult);

		member.Update($"{data.FirstName.Trim()} {data.LastName.Trim()}", data.BirthDate, data.GuardianName?.Trim(), data.GuardianTaxId?.Trim(), member.ParQSigned);
		await db.SaveChangesAsync(cancellationToken);
		await transaction.CommitAsync(cancellationToken);
		return Result.Success();
	}

	public async Task<Result> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword, CancellationToken cancellationToken = default)
	{
		var user = await userManager.FindByIdAsync(userId.ToString());
		if (user is null) return Result.Failure("not_found", "Usuário não encontrado.");
		var result = await userManager.ChangePasswordAsync(user, currentPassword, newPassword);
		return result.Succeeded ? Result.Success() : Failure(result);
	}

	public async Task RequestPasswordResetAsync(string email, CancellationToken cancellationToken = default)
	{
		var user = await userManager.FindByEmailAsync(email.Trim());
		if (user is null) return;
		var token = await userManager.GeneratePasswordResetTokenAsync(user);
		await passwordResetEmailSender.SendAsync(user.Email!, token, cancellationToken);
	}

	public async Task<Result> ResetPasswordAsync(string email, string token, string newPassword, CancellationToken cancellationToken = default)
	{
		var user = await userManager.FindByEmailAsync(email.Trim());
		if (user is null) return Result.Failure("invalid_token", "Token de recuperação inválido ou expirado.");
		var result = await userManager.ResetPasswordAsync(user, token, newPassword);
		return result.Succeeded ? Result.Success() : Failure(result, "invalid_token");
	}

	public async Task<Result> LogoutAsync(Guid userId, string? tokenId, DateTime? tokenExpiresAtUtc, CancellationToken cancellationToken = default)
	{
		if (!string.IsNullOrWhiteSpace(tokenId) && tokenExpiresAtUtc is not null)
		{
			var duration = tokenExpiresAtUtc.Value - DateTime.UtcNow;
			if (duration > TimeSpan.Zero) await tokenBlacklist.RevokeAsync(tokenId, duration, cancellationToken);
		}
		return Result.Success();
	}

	private AuthenticationResult CreateAuthenticationResult(ApplicationUser user, IEnumerable<string> roles)
	{
		var roleList = roles.ToArray();
		var token = tokenService.GenerateToken(new TokenSubject(user.Id, user.UserName!, user.Email!, roleList));
		return new AuthenticationResult(token.AccessToken, token.ExpiresAtUtc, user.UserName!, roleList);
	}

	private static Result Failure(IdentityResult result, string code = "identity_error") => Result.Failure(code, string.Join(" ", result.Errors.Select(error => error.Description)));
	private static Result<T> Failure<T>(IdentityResult result, string code = "identity_error") => Result<T>.Failure(code, string.Join(" ", result.Errors.Select(error => error.Description)));
}
