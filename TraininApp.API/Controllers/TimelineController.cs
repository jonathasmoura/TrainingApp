using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraininApp.Application.Interfaces;

namespace TraininApp.API.Controllers;

[ApiController]
[Authorize]
[ApiExplorerSettings(GroupName = "timeline")]
[Route("api/timeline")]
public sealed class TimelineController(ITimelineService timelineService) : ControllerBase
{
	/// <summary>Consulta o histórico de atividades de um membro.</summary>
	[HttpGet("members/{memberId:guid}")]
	public async Task<IActionResult> GetMemberTimeline(
		Guid memberId,
		[FromQuery] DateTime? from,
		[FromQuery] DateTime? to,
		[FromQuery] string? type,
		[FromQuery] int page = 1,
		[FromQuery] int pageSize = 20,
		CancellationToken cancellationToken = default)
	{
		if (page < 1 || pageSize is < 1 or > 100)
			return BadRequest("page deve ser maior que zero e pageSize deve estar entre 1 e 100.");

		var items = await timelineService.GetMemberTimelineAsync(memberId, from, to, type, page, pageSize, cancellationToken);
		return Ok(new { items, page, pageSize });
	}

	/// <summary>Consulta o histórico de atividades de um profissional.</summary>
	[HttpGet("professionals/{professionalId:guid}")]
	public async Task<IActionResult> GetProfessionalTimeline(
		Guid professionalId,
		[FromQuery] DateTime? from,
		[FromQuery] DateTime? to,
		[FromQuery] string? type,
		[FromQuery] int page = 1,
		[FromQuery] int pageSize = 20,
		CancellationToken cancellationToken = default)
	{
		if (page < 1 || pageSize is < 1 or > 100)
			return BadRequest("page deve ser maior que zero e pageSize deve estar entre 1 e 100.");

		var items = await timelineService.GetProfessionalTimelineAsync(professionalId, from, to, type, page, pageSize, cancellationToken);
		return Ok(new { items, page, pageSize });
	}
}
