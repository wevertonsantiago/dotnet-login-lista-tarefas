namespace dto;

public enum TaskOrderQueryEnum
{
    dateCreate,
    dateUpgrade
}

public class TaskOrderQueryDTO
{
    public TaskOrderQueryEnum OrderBy { get; set; }
    public bool Reverse { get; set; }
    public string? Search { get; set; }

}


