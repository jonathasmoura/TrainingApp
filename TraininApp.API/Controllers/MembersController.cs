using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Metadata;
using TraininApp.Application.Abstractions;
using TraininApp.Application.Features.Members;

namespace TraininApp.API.Controllers;

[ApiController]
[Route("api/v1/members")]
[ApiExplorerSettings(GroupName = "members")]
[Authorize]
public sealed class MembersController(IDispatcher dispatcher) : ControllerBase
{
	[HttpGet]
	[Authorize(Roles = "Admin,Professional")]
	[EndpointSummary("Listar membros")]
	[EndpointDescription("Retorna a lista de membros cadastrados.")]
	public Task<IReadOnlyList<TraininApp.Application.Dtos.MemberDto>> List(CancellationToken cancellationToken) => dispatcher.Query(new ListMembersQuery(), cancellationToken);

	[HttpGet("{id:guid}")]
	[Authorize(Roles = "Admin,Professional")]
	[EndpointSummary("Obter membro")]
	[EndpointDescription("Retorna os dados de um membro específico pelo identificador.")]
	public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
	{
		var result = await dispatcher.Query(new GetMemberByIdQuery(id), cancellationToken);
		return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
	}

	[HttpPut("{id:guid}")]
	[EndpointSummary("Atualizar membro")]
	[EndpointDescription("Atualiza os dados de um membro existente.")]
	public IActionResult Update(Guid id) => StatusCode(StatusCodes.Status501NotImplemented);

	[HttpDelete("{id:guid}")]
	[EndpointSummary("Excluir membro")]
	[EndpointDescription("Exclui um membro cadastrado.")]
	public IActionResult Delete(Guid id) => StatusCode(StatusCodes.Status501NotImplemented);
}
