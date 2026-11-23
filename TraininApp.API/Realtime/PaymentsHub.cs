using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace TraininApp.API.Realtime;

[Authorize]
public sealed class PaymentsHub : Hub
{
	public Task JoinReceptionMonitor()
	{
		if (!Context.User?.IsInRole("Admin") == true && !Context.User?.IsInRole("Professional") == true) throw new HubException("Access denied.");
		return Groups.AddToGroupAsync(Context.ConnectionId, "ReceptionMonitor");
	}

	public Task JoinUser(Guid userId)
	{
		var isOwner = Context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value == userId.ToString();
		if (!isOwner && !Context.User?.IsInRole("Admin") == true && !Context.User?.IsInRole("Professional") == true) throw new HubException("Access denied.");
		return Groups.AddToGroupAsync(Context.ConnectionId, $"User_{userId}");
	}
}
