using TraininApp.Application.Abstractions;
using TraininApp.Application.Common;
using TraininApp.Domain.Enums;

namespace TraininApp.Application.Features.Installments;

public sealed record SettleInstallmentCommand(Guid Id, EPaymentMethod PaymentMethod) : ICommand<Result>;

