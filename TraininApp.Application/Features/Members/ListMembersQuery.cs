using TraininApp.Application.Abstractions;
using TraininApp.Application.Dtos;

namespace TraininApp.Application.Features.Members;

public sealed record ListMembersQuery : IQuery<IReadOnlyList<MemberDto>>;

