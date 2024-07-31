namespace TaskMate.DTOs.Checkitem;

public class GetCheckItemCountDto
{
    public DateTime CreatedDate { get; set; }
    public DateTime ModiffiedDate { get; set; } 
    public virtual bool isDeleted { get; set; }
    public int Done { get; set; }  // Renaming 'True' to 'Done' for clarity
    public int Total { get; set; } // Total number of tasks
}

