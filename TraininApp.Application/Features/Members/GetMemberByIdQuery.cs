using TraininApp.Application.Abstractions;
using TraininApp.Application.Common;
using TraininApp.Application.Dtos;

namespace TraininApp.Application.Features.Members;

public sealed record GetMemberByIdQuery(Guid Id) : IQuery<Result<MemberDto>>;

