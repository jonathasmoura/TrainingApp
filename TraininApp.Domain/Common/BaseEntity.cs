namespace TraininApp.Domain.Common;

public abstract class BaseEntity
{
	protected BaseEntity() { }
	protected BaseEntity(Guid id) => Id = id;
	public Guid Id { get; protected set; } = Guid.CreateVersion7();
	public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
	public DateTime? UpdatedAt { get; protected set; }
	public bool Active { get; protected set; } = true;
	public void Desativar() { Active = false; UpdatedAt = DateTime.UtcNow; }
	public void Reativar() { Active = true; UpdatedAt = DateTime.UtcNow; }
}

