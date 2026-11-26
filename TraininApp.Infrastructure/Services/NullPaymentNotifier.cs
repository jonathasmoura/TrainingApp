using TraininApp.Application.Interfaces;

namespace TraininApp.Infrastructure.Services;

public sealed class NullPaymentNotifier : IPaymentNotifier
{
	public Task NotifyAsync(Guid memberId, Guid installmentId, CancellationToken cancellationToken = default) => Task.CompletedTask;
}

