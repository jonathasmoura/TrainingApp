using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Metadata;
using TraininApp.Application.Abstractions;
using TraininApp.Application.Features.Enrollments;

namespace TraininApp.API.Controllers;

[ApiController]
[Route("api/v1/enrollments")]
[ApiExplorerSettings(GroupName = "enrollments")]
[Authorize(Roles = "Admin,Professional")]
public sealed class EnrollmentsController(IDispatcher dispatcher) : ControllerBase
{
	[HttpPost]
	[EndpointSummary("Criar matrícula")]
	[EndpointDescription("Matricula um membro em um plano de treinamento.")]
	public async Task<IActionResult> Create(CreateEnrollmentCommand command, CancellationToken cancellationToken)
	{
		var result = await dispatcher.Send(command, cancellationToken);
		return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
	}

	[HttpPut("{id:guid}/switch-plan")]
	[EndpointSummary("Alterar plano da matrícula")]
	[EndpointDescription("Altera o plano associado a uma matrícula existente.")]
	public IActionResult SwitchPlan(Guid id) => StatusCode(StatusCodes.Status501NotImplemented);

	[HttpDelete("{id:guid}")]
	[EndpointSummary("Cancelar matrícula")]
	[EndpointDescription("Cancela uma matrícula existente de um membro.")]
	public IActionResult Delete(Guid id) => StatusCode(StatusCodes.Status501NotImplemented);
}
