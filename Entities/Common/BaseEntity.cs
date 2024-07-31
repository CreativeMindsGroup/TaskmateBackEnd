namespace TaskMate.Entities.Common;

public abstract class BaseEntity
{
    public Guid Id { get; set; } = new Guid();
    public DateTime CreatedDate { get; set; } 
    public DateTime ModiffiedDate { get; set; } 
    public virtual bool isDeleted { get; set; }
}