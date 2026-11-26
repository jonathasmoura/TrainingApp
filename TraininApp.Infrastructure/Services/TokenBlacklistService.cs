using Microsoft.Extensions.Caching.Distributed;
using TraininApp.Application.Interfaces;

namespace TraininApp.Infrastructure.Services;

public sealed class TokenBlacklistService(IDistributedCache cache) : ITokenBlacklistService
{
	public async Task<bool> IsRevokedAsync(string jti, CancellationToken cancellationToken = default) => await cache.GetStringAsync($"revoked:{jti}", cancellationToken) is not null;
	public Task RevokeAsync(string jti, TimeSpan duration, CancellationToken cancellationToken = default) => cache.SetStringAsync($"revoked:{jti}", "1", new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = duration }, cancellationToken);
}

