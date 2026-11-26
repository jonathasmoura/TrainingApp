namespace TraininApp.Domain.Entities;

public sealed class TimelineEntry
{
	private TimelineEntry() { }

	public TimelineEntry(
		Guid? memberId,
		Guid? professionalId,
		string eventType,
		string title,
		string description,
		DateTime occurredAtUtc,
		Guid? relatedEntityId = null,
		string? relatedEntityType = null)
	{
		Id = Guid.NewGuid();
		MemberId = memberId;
		ProfessionalId = professionalId;
		EventType = eventType;
		Title = title;
		Description = description;
		OccurredAtUtc = occurredAtUtc;
		RelatedEntityId = relatedEntityId;
		RelatedEntityType = relatedEntityType;
	}

	public Guid Id { get; private set; }
	public Guid? MemberId { get; private set; }
	public Guid? ProfessionalId { get; private set; }
	public string EventType { get; private set; } = null!;
	public string Title { get; private set; } = null!;
	public string Description { get; private set; } = null!;
	public DateTime OccurredAtUtc { get; private set; }
	public Guid? RelatedEntityId { get; private set; }
	public string? RelatedEntityType { get; private set; }
}
