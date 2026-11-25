using Microsoft.Extensions.Logging;
using TraininApp.Application.Interfaces;

namespace TraininApp.Infrastructure.Services;

public sealed class PasswordResetEmailSender(ILogger<PasswordResetEmailSender> logger) : IPasswordResetEmailSender
{
	public Task SendAsync(string email, string token, CancellationToken cancellationToken = default)
	{
		logger.LogInformation("Password reset requested for {Email}. Configure a production email provider before enabling external delivery.", email);
		return Task.CompletedTask;
	}
}
