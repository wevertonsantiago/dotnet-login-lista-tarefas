namespace dto;

public class TaskCreateResDTO
{

    public required String Title { get; set; }
    public required String Description { get; set; }
    public DateTime DateCreate { get; set; }

}