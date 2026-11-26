namespace TraininApp.Application.Interfaces;

public interface IIdempotencyService
{
	Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default);
	Task StoreAsync(string key, TimeSpan duration, CancellationToken cancellationToken = default);
}

