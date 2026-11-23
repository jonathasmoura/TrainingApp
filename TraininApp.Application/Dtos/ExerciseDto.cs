namespace TraininApp.Application.Dtos;

public sealed record ExerciseDto(Guid Id, string Name, string PrimaryMuscleGroup, string Equipment, string Difficulty, string? VideoUrl);

