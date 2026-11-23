using Microsoft.Extensions.Caching.Distributed;
using TraininApp.Application.Interfaces;

namespace TraininApp.Infrastructure.Services;

public sealed class IdempotencyService(IDistributedCache cache) : IIdempotencyService
{
	public async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default) => await cache.GetStringAsync($"idempotency:{key}", cancellationToken) is not null;
	public Task StoreAsync(string key, TimeSpan duration, CancellationToken cancellationToken = default) => cache.SetStringAsync($"idempotency:{key}", "1", new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = duration }, cancellationToken);
}

