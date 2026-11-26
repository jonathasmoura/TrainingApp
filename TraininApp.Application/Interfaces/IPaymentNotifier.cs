namespace TraininApp.Application.Interfaces;

public interface IPaymentNotifier
{
	Task NotifyAsync(Guid memberId, Guid installmentId, CancellationToken cancellationToken = default);
}

