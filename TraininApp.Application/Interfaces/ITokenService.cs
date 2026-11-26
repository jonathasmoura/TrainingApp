namespace TraininApp.Application.Interfaces;

public interface ITokenService
{
	TokenResult GenerateToken(TokenSubject subject);
}

public sealed record TokenSubject(Guid UserId, string UserName, string Email, IEnumerable<string> Roles);
public sealed record TokenResult(string AccessToken, DateTime ExpiresAtUtc);
