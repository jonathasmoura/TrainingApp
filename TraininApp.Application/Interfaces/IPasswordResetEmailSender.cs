namespace TraininApp.Application.Interfaces;

public interface IPasswordResetEmailSender
{
	Task SendAsync(string email, string token, CancellationToken cancellationToken = default);
}
