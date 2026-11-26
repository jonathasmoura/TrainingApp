using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Metadata;

namespace TraininApp.API.Controllers;

[ApiController]
[Route("api/v1/workout-plans")]
[ApiExplorerSettings(GroupName = "workout-plans")]
[Authorize]
public sealed class WorkoutPlansController : ControllerBase
{
	[HttpGet("member/{memberId:guid}/active")]
	[EndpointSummary("Obter ficha ativa do membro")]
	[EndpointDescription("Retorna a ficha de treino ativa associada a um membro.")]
	public IActionResult GetActiveByMember(Guid memberId) => StatusCode(StatusCodes.Status501NotImplemented);

	[HttpPost]
	[Authorize(Roles = "Admin,Professional")]
	[EndpointSummary("Criar ficha de treino")]
	[EndpointDescription("Cadastra uma nova ficha de treino para um membro.")]
	public IActionResult Create() => StatusCode(StatusCodes.Status501NotImplemented);

	[HttpPut("{id:guid}")]
	[Authorize(Roles = "Admin,Professional")]
	[EndpointSummary("Atualizar ficha de treino")]
	[EndpointDescription("Atualiza uma ficha de treino existente.")]
	public IActionResult Update(Guid id) => StatusCode(StatusCodes.Status501NotImplemented);
}
