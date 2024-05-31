namespace TaskMate.Entities.Common;

public abstract class BaseEntity
{
    public Guid Id { get; set; } = new Guid();
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime ModiffiedDate { get; set; } = DateTime.UtcNow;
    public virtual bool isDeleted { get; set; }
}