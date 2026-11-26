using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Metadata;
using TraininApp.Application.Abstractions;
using TraininApp.Application.Features.Plans;

namespace TraininApp.API.Controllers;

[ApiController]
[Route("api/v1/plans")]
[ApiExplorerSettings(GroupName = "plans")]
[Authorize(Roles = "Admin,Professional")]
public sealed class PlansController(IDispatcher dispatcher) : ControllerBase
{
	[HttpGet]
	[EndpointSummary("Listar planos")]
	[EndpointDescription("Retorna a lista de planos de treinamento cadastrados.")]
	public IActionResult List() => StatusCode(StatusCodes.Status501NotImplemented);

	[HttpPost]
	[EndpointSummary("Criar plano")]
	[EndpointDescription("Cadastra um novo plano de treinamento.")]
	public async Task<IActionResult> Create(CreatePlanCommand command, CancellationToken cancellationToken)
	{
		var result = await dispatcher.Send(command, cancellationToken);
		return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
	}

	[HttpPut("{id:guid}")]
	[EndpointSummary("Atualizar plano")]
	[EndpointDescription("Atualiza os dados de um plano de treinamento existente.")]
	public IActionResult Update(Guid id) => StatusCode(StatusCodes.Status501NotImplemented);

	[HttpDelete("{id:guid}")]
	[EndpointSummary("Excluir plano")]
	[EndpointDescription("Exclui um plano de treinamento cadastrado.")]
	public IActionResult Delete(Guid id) => StatusCode(StatusCodes.Status501NotImplemented);
}
