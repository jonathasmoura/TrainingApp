namespace TraininApp.Application.Dtos;

public sealed record ProfessionalDto(Guid Id, string Name, string TaxId, string LicenseNumber, DateOnly LicenseExpirationDate, string Status);

