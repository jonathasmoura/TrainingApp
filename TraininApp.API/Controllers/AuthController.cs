using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Metadata;
using TraininApp.API.Contracts;
using TraininApp.Application.Interfaces;
using TraininApp.Infrastructure.Persistence;

namespace TraininApp.API.Controllers;

[ApiController]
[Route("api/v1/auth")]
[ApiExplorerSettings(GroupName = "auth")]
public sealed class AuthController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, ITokenService tokenService) : ControllerBase
{
	[HttpPost("register")]
	[EndpointSummary("Registrar usuário")]
	[EndpointDescription("Cria uma nova conta de usuário na plataforma.")]
	public async Task<IActionResult> Register(RegisterRequest request, CancellationToken cancellationToken)
	{
		var user = new ApplicationUser
		{
			UserName = request.UserName,
			Email = request.Email,
			PhoneNumber = request.MobilePhone,
			FirstName = request.FirstName,
			LastName = request.LastName,
			AlternatePhone = request.AlternatePhone
		};
		var result = await userManager.CreateAsync(user, request.Password);
		if (!result.Succeeded) return BadRequest(new { errors = result.Errors.Select(error => error.Description) });

		if (!await roleManager.RoleExistsAsync("Member")) await roleManager.CreateAsync(new IdentityRole<Guid>("Member"));
		var roleResult = await userManager.AddToRoleAsync(user, "Member");
		if (!roleResult.Succeeded)
		{
			await userManager.DeleteAsync(user);
			return StatusCode(StatusCodes.Status500InternalServerError, new { errors = roleResult.Errors.Select(error => error.Description) });
		}

		return StatusCode(StatusCodes.Status201Created, CreateResponse(user, ["Member"]));
	}

	[HttpPost("login")]
	[EndpointSummary("Autenticar usuário")]
	[EndpointDescription("Valida as credenciais do usuário e inicia uma sessão autenticada.")]
	public async Task<IActionResult> Login(LoginRequest request)
	{
		var user = await userManager.FindByNameAsync(request.UserName);
		if (user is null || !await userManager.CheckPasswordAsync(user, request.Password)) return Unauthorized(new { error = "Invalid username or password." });

		var roles = await userManager.GetRolesAsync(user);
		return Ok(CreateResponse(user, roles));
	}

	private AuthenticationResponse CreateResponse(ApplicationUser user, IEnumerable<string> roles)
	{
		var roleList = roles.ToArray();
		var token = tokenService.GenerateToken(new TokenSubject(user.Id, user.UserName!, user.Email!, roleList));
		return new AuthenticationResponse(token.AccessToken, token.ExpiresAtUtc, user.UserName!, roleList);
	}

	[HttpPost("refresh-token")]
	[EndpointSummary("Atualizar token")]
	[EndpointDescription("Gera um novo token de acesso a partir de um refresh token válido.")]
	public IActionResult RefreshToken() => StatusCode(StatusCodes.Status501NotImplemented);

	[HttpPost("logout")]
	[EndpointSummary("Encerrar sessão")]
	[EndpointDescription("Encerra a sessão autenticada do usuário.")]
	public IActionResult Logout() => StatusCode(StatusCodes.Status501NotImplemented);
}
