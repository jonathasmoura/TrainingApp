using TraininApp.Application.Abstractions;
using TraininApp.Application.Common;
using TraininApp.Application.Dtos;
using TraininApp.Application.Interfaces;

namespace TraininApp.Application.Features.Members;

public sealed class GetMemberByIdHandler(IMemberRepository repository) : IQueryHandler<GetMemberByIdQuery, Result<MemberDto>>
{
	public async Task<Result<MemberDto>> Handle(GetMemberByIdQuery query, CancellationToken cancellationToken)
	{
		var member = await repository.GetByIdAsync(query.Id, cancellationToken);
		return member is null
			? Result<MemberDto>.Failure("not_found", "Membro não encontrado.")
			: Result<MemberDto>.Success(new MemberDto(member.Id, member.Name, member.TaxId, member.BirthDate, member.ParQSigned));
	}
}

