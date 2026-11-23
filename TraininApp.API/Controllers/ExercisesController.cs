using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Metadata;

namespace TraininApp.API.Controllers;

[ApiController]
[Route("api/v1/exercises")]
[ApiExplorerSettings(GroupName = "exercises")]
[Authorize(Roles = "Admin,Professional")]
public sealed class ExercisesController : ControllerBase
{
	[HttpGet]
	[EndpointSummary("Listar exercícios")]
	[EndpointDescription("Retorna a lista de exercícios cadastrados.")]
	public IActionResult List() => StatusCode(StatusCodes.Status501NotImplemented);

	[HttpPost]
	[EndpointSummary("Criar exercício")]
	[EndpointDescription("Cadastra um novo exercício para uso nos treinos.")]
	public IActionResult Create() => StatusCode(StatusCodes.Status501NotImplemented);

	[HttpPut("{id:guid}")]
	[EndpointSummary("Atualizar exercício")]
	[EndpointDescription("Atualiza os dados de um exercício existente.")]
	public IActionResult Update(Guid id) => StatusCode(StatusCodes.Status501NotImplemented);

	[HttpDelete("{id:guid}")]
	[EndpointSummary("Excluir exercício")]
	[EndpointDescription("Exclui um exercício cadastrado.")]
	public IActionResult Delete(Guid id) => StatusCode(StatusCodes.Status501NotImplemented);
}
