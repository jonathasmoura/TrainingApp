using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraininApp.API.Contracts;
using TraininApp.Application.Interfaces;

namespace TraininApp.API.Controllers;

/// <summary>
/// Endpoints para cadastro, autenticação, renovação de tokens e encerramento de sessões de usuários.
/// </summary>
[ApiController]
[Route("api/v1/auth")]
[ApiExplorerSettings(GroupName = "auth")]
public sealed class AuthController(IIdentityService identityService) : ControllerBase
{
	/// <summary>
	/// Cria uma nova conta de usuário.
	/// </summary>
	/// <param name="request">Os dados de registro contendo os detalhes do usuário.</param>
	/// <param name="cancellationToken">O token de cancelamento.</param>
	/// <response code="201">Retorna o perfil do usuário criado.</response>
	/// <response code="400">Se os dados de registro forem inválidos.</response>
	[HttpPost("register")]
	public async Task<IActionResult> Register(RegisterRequest request, CancellationToken cancellationToken)
	{
		var result = await identityService.RegisterAsync(new TraininApp.Application.Interfaces.RegisterData(request.UserName, request.FirstName, request.LastName, request.Email, request.MobilePhone, request.AlternatePhone, request.Password, request.TaxId, request.BirthDate, request.GuardianName, request.GuardianTaxId), cancellationToken);
		return result.IsSuccess ? StatusCode(StatusCodes.Status201Created, result.Value) : BadRequest(result.Error);
	}

	/// <summary>
	/// Valida as credenciais do usuário e retorna um token de acesso.
	/// </summary>
	/// <param name="request">A solicitação de login contendo as credenciais do usuário.</param>
	/// <param name="cancellationToken">O token de cancelamento.</param>
	/// <response code="200">Retorna o token de acesso e as informações do usuário.</response>
	/// <response code="401">Se as credenciais forem inválidas.</response>
	[HttpPost("login")]
	public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
	{
		var result = await identityService.LoginAsync(new TraininApp.Application.Interfaces.LoginData(request.UserName, request.Password), cancellationToken);
		return result.IsSuccess ? Ok(result.Value) : Unauthorized(result.Error);
	}

	/// <summary>
	/// Solicita o envio de um link para redefinição de senha para o e-mail do usuário.
	/// </summary>
	/// <param name="request">A solicitação de esqueci a senha contendo o e-mail do usuário.</param>
	/// <param name="cancellationToken">O token de cancelamento.</param>
	/// <response code="202">Solicitação de redefinição de senha aceita.</response>
	[HttpPost("forgot-password")]
	public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request, CancellationToken cancellationToken)
	{
		await identityService.RequestPasswordResetAsync(request.Email, cancellationToken);
		return Accepted();
	}

	/// <summary>
	/// Redefine a senha do usuário para um novo valor.
	/// </summary>
	/// <param name="request">A solicitação de redefinição de senha contendo e-mail, token e nova senha.</param>
	/// <param name="cancellationToken">O token de cancelamento.</param>
	/// <response code="204">Indica que a senha foi redefinida com sucesso.</response>
	/// <response code="400">Se os dados de redefinição forem inválidos.</response>
	[HttpPost("reset-password")]
	public async Task<IActionResult> ResetPassword(ResetPasswordRequest request, CancellationToken cancellationToken)
	{
		var result = await identityService.ResetPasswordAsync(request.Email, request.Token, request.NewPassword, cancellationToken);
		return result.IsSuccess ? NoContent() : BadRequest(result.Error);
	}

	/// <summary>
	/// Altera a senha do usuário.
	/// </summary>
	/// <param name="request">A solicitação de alteração de senha contendo a senha atual e a nova senha.</param>
	/// <param name="cancellationToken">O token de cancelamento.</param>
	/// <response code="204">Indica que a senha foi alterada com sucesso.</response>
	/// <response code="400">Se os dados de alteração de senha forem inválidos.</response>
	/// <response code="401">Se a senha atual estiver incorreta.</response>
	[Authorize]
	[HttpPost("change-password")]
	public async Task<IActionResult> ChangePassword(ChangePasswordRequest request, CancellationToken cancellationToken)
	{
		var result = await identityService.ChangePasswordAsync(GetUserId(), request.CurrentPassword, request.NewPassword, cancellationToken);
		return result.IsSuccess ? NoContent() : BadRequest(result.Error);
	}

	/// <summary>
	/// Atualiza as informações do perfil do usuário.
	/// </summary>
	/// <param name="request">A solicitação de atualização de perfil contendo os novos dados do perfil.</param>
	/// <param name="cancellationToken">O token de cancelamento.</param>
	/// <response code="204">Indica que o perfil foi atualizado com sucesso.</response>
	/// <response code="400">Se os dados de atualização do perfil forem inválidos.</response>
	[Authorize]
	[HttpPut("me")]
	public async Task<IActionResult> UpdateProfile(UpdateProfileRequest request, CancellationToken cancellationToken)
	{
		var result = await identityService.UpdateProfileAsync(GetUserId(), new ProfileUpdateData(request.FirstName, request.LastName, request.MobilePhone, request.AlternatePhone, request.BirthDate, request.GuardianName, request.GuardianTaxId), cancellationToken);
		return result.IsSuccess ? NoContent() : BadRequest(result.Error);
	}

	/// <summary>
	/// Encerra a sessão e invalida o refresh token atual.
	/// </summary>
	/// <param name="cancellationToken">O token de cancelamento.</param>
	/// <response code="204">Indica que o usuário foi desconectado com sucesso.</response>
	/// <response code="400">Se a solicitação deLogout  for inválida.</response>
	[Authorize]
	[HttpPost("logout")]
	public async Task<IActionResult> Logout(CancellationToken cancellationToken)
	{
		var expiresAt = User.FindFirstValue(JwtRegisteredClaimNames.Exp) is { } exp && long.TryParse(exp, out var seconds)
			? DateTimeOffset.FromUnixTimeSeconds(seconds).UtcDateTime
			: (DateTime?)null;
		var result = await identityService.LogoutAsync(GetUserId(), User.FindFirstValue(JwtRegisteredClaimNames.Jti), expiresAt, cancellationToken);
		return result.IsSuccess ? NoContent() : BadRequest(result.Error);
	}

	private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
