using TraininApp.Application.Abstractions;
using TraininApp.Application.Common;
using TraininApp.Application.Interfaces;
using TraininApp.Domain.Services;

namespace TraininApp.Application.Features.Installments;

public sealed class SettleInstallmentHandler(IInstallmentRepository installments, IUnitOfWork unitOfWork, IEnrollmentDomainService service, IPaymentNotifier notifier) : ICommandHandler<SettleInstallmentCommand, Result>
{
	public async Task<Result> Handle(SettleInstallmentCommand command, CancellationToken cancellationToken)
	{
		var installment = await installments.GetWithEnrollmentAsync(command.Id, cancellationToken);
		if (installment is null) return Result.Failure("not_found", "Installment not found.");
		service.SettleInstallment(installment, command.PaymentMethod);
		await unitOfWork.SaveChangesAsync(cancellationToken);
		await notifier.NotifyAsync(installment.EnrollmentId, installment.Id, cancellationToken);
		return Result.Success();
	}
}

