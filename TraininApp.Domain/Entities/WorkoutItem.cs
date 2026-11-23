using TraininApp.Domain.Common;

namespace TraininApp.Domain.Entities;

public sealed class WorkoutItem : BaseEntity
{
	private WorkoutItem() { }
	public WorkoutItem(Guid workoutPlanId, Guid exerciseId, string split, int sets, int repetitions, int restSeconds) { WorkoutPlanId = workoutPlanId; ExerciseId = exerciseId; Split = split; Sets = sets; Repetitions = repetitions; RestSeconds = restSeconds; }
	public Guid WorkoutPlanId { get; private set; }
	public Guid ExerciseId { get; private set; }
	public string Split { get; private set; } = string.Empty;
	public int Sets { get; private set; }
	public int Repetitions { get; private set; }
	public int RestSeconds { get; private set; }
}

