namespace TaskMate.DTOs.Checkitem;

public class GetCheckItemCountDto
{
    public int Done { get; set; }  // Renaming 'True' to 'Done' for clarity
    public int Total { get; set; } // Total number of tasks
}

