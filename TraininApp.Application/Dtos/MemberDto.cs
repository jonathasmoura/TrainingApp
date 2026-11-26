namespace TraininApp.Application.Dtos;

public sealed record MemberDto(Guid Id, string Name, string TaxId, DateOnly BirthDate, bool ParQSigned);

