namespace TraininApp.Application.Interfaces;

public interface ITokenBlacklistService
{
	Task<bool> IsRevokedAsync(string jti, CancellationToken cancellationToken = default);
	Task RevokeAsync(string jti, TimeSpan duration, CancellationToken cancellationToken = default);
}

