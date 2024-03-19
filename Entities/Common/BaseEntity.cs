namespace TaskMate.Entities.Common;

public abstract class BaseEntity
{
    public Guid Id { get; set; } = new Guid();
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime ModiffiedDate { get; set; } = DateTime.Now;
    public virtual bool isDeleted { get; set; }
}