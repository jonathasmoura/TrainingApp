using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TraininApp.Application.Interfaces;

namespace TraininApp.Infrastructure.Authentication;

public sealed class JwtTokenService(IOptions<JwtOptions> options) : ITokenService
{
	private readonly JwtOptions _options = options.Value;

	public TokenResult GenerateToken(TokenSubject subject)
	{
		var expiresAtUtc = DateTime.UtcNow.AddMinutes(_options.ExpirationMinutes);
		var claims = new List<Claim>
		{
			new(JwtRegisteredClaimNames.Sub, subject.UserId.ToString()),
			new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			new(ClaimTypes.NameIdentifier, subject.UserId.ToString()),
			new(ClaimTypes.Name, subject.UserName),
			new(ClaimTypes.Email, subject.Email)
		};

		claims.AddRange(subject.Roles.Select(role => new Claim(ClaimTypes.Role, role)));
		var credentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key)), SecurityAlgorithms.HmacSha256);
		var token = new JwtSecurityToken(_options.Issuer, _options.Audience, claims, expires: expiresAtUtc, signingCredentials: credentials);

		return new TokenResult(new JwtSecurityTokenHandler().WriteToken(token), expiresAtUtc);
	}
}
