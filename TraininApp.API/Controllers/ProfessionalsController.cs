using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Metadata;

namespace TraininApp.API.Controllers;

[ApiController]
[Route("api/v1/professionals")]
[ApiExplorerSettings(GroupName = "professionals")]
[Authorize(Roles = "Admin,Professional")]
public sealed class ProfessionalsController : ControllerBase
{
	[HttpGet]
	[EndpointSummary("Listar profissionais")]
	[EndpointDescription("Retorna a lista de profissionais cadastrados.")]
	public IActionResult List() => StatusCode(StatusCodes.Status501NotImplemented);

	[HttpPut("{id:guid}")]
	[EndpointSummary("Atualizar profissional")]
	[EndpointDescription("Atualiza os dados de um profissional existente.")]
	public IActionResult Update(Guid id) => StatusCode(StatusCodes.Status501NotImplemented);

	[HttpDelete("{id:guid}")]
	[EndpointSummary("Excluir profissional")]
	[EndpointDescription("Exclui um profissional cadastrado.")]
	public IActionResult Delete(Guid id) => StatusCode(StatusCodes.Status501NotImplemented);
}
