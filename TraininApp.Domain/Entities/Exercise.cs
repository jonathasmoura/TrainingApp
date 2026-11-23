using TraininApp.Domain.Common;
using TraininApp.Domain.Enums;

namespace TraininApp.Domain.Entities;

public sealed class Exercise : BaseEntity
{
	private Exercise() { }
	public Exercise(string name, EMuscleGroup primaryMuscleGroup, string equipment, EDifficultyLevel difficulty, string? videoUrl = null) { Name = name; PrimaryMuscleGroup = primaryMuscleGroup; Equipment = equipment; Difficulty = difficulty; VideoUrl = videoUrl; }
	public string Name { get; private set; } = string.Empty;
	public EMuscleGroup PrimaryMuscleGroup { get; private set; }
	public string Equipment { get; private set; } = string.Empty;
	public EDifficultyLevel Difficulty { get; private set; }
	public string? VideoUrl { get; private set; }
	public void Update(string name, EMuscleGroup primaryMuscleGroup, string equipment, EDifficultyLevel difficulty, string? videoUrl) { Name = name; PrimaryMuscleGroup = primaryMuscleGroup; Equipment = equipment; Difficulty = difficulty; VideoUrl = videoUrl; UpdatedAt = DateTime.UtcNow; }
}

