using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TraininApp.Application.Abstractions;

namespace TraininApp.Infrastructure.CQRS;

public sealed class Dispatcher(IServiceProvider services) : IDispatcher
{
	public async Task<TResponse> Send<TResponse>(ICommand<TResponse> command, CancellationToken ct = default)
	{
		var validatorType = typeof(IValidator<>).MakeGenericType(command.GetType());
		var validators = (IEnumerable<object>?)services.GetService(typeof(IEnumerable<>).MakeGenericType(validatorType)) ?? [];
		foreach (var validator in validators)
		{
			var result = await (Task<FluentValidation.Results.ValidationResult>)validatorType.GetMethod("ValidateAsync", [typeof(IValidationContext), typeof(CancellationToken)])!.Invoke(validator, [new ValidationContext<object>(command), ct])!;
			if (!result.IsValid) throw new ValidationException(result.Errors);
		}
		var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResponse));
		dynamic handler = services.GetRequiredService(handlerType);
		return await handler.Handle((dynamic)command, ct);
	}
	public async Task<TResponse> Query<TResponse>(IQuery<TResponse> query, CancellationToken ct = default)
	{ var type = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResponse)); dynamic handler = services.GetRequiredService(type); return await handler.Handle((dynamic)query, ct); }
}

