using TraininApp.Application.Abstractions;
using TraininApp.Application.Dtos;
using TraininApp.Application.Interfaces;

namespace TraininApp.Application.Features.Members;

public sealed class ListMembersHandler(IMemberRepository repository) : IQueryHandler<ListMembersQuery, IReadOnlyList<MemberDto>>
{
	public async Task<IReadOnlyList<MemberDto>> Handle(ListMembersQuery _, CancellationToken cancellationToken) =>
		(await repository.ListAsync(cancellationToken)).Select(a => new MemberDto(a.Id, a.Name, a.TaxId, a.BirthDate, a.ParQSigned)).ToList();
}

