using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Metadata;
using TraininApp.Application.Abstractions;
using TraininApp.Application.Features.Installments;
using TraininApp.Domain.Enums;

namespace TraininApp.API.Controllers;

[ApiController]
[Route("api/v1/installments")]
[ApiExplorerSettings(GroupName = "installments")]
[Authorize(Roles = "Admin,Professional")]
public sealed class InstallmentsController(IDispatcher dispatcher) : ControllerBase
{
	[HttpGet("pending")]
	[EndpointSummary("Listar parcelas pendentes")]
	[EndpointDescription("Retorna as parcelas financeiras que ainda aguardam pagamento.")]
	public IActionResult ListPending() => StatusCode(StatusCodes.Status501NotImplemented);

	[HttpPost("{id:guid}/settle-payment")]
	[EndpointSummary("Liquidar parcela")]
	[EndpointDescription("Registra o pagamento de uma parcela usando o método informado.")]
	public async Task<IActionResult> SettlePayment(Guid id, [FromQuery] EPaymentMethod paymentMethod, [FromHeader(Name = "Idempotency-Key")] string? idempotencyKey, CancellationToken cancellationToken)
	{
		if (string.IsNullOrWhiteSpace(idempotencyKey)) return BadRequest(new { error = "Idempotency-Key is required." });
		var result = await dispatcher.Send(new SettleInstallmentCommand(id, paymentMethod), cancellationToken);
		return result.IsSuccess ? NoContent() : BadRequest(result.Error);
	}
}
