namespace TraininApp.Application.Dtos;

public sealed record InstallmentDto(Guid Id, Guid EnrollmentId, int InstallmentNumber, DateOnly DueDate, decimal Amount, string Status);

