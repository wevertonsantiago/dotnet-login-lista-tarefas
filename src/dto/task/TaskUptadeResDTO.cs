
namespace dto;

public class TaskUptadeResDTO
{
    public required String Title { get; set; }
    public required String Description { get; set; }
    public DateTime DateCreate { get; set; }
    public DateTime DateUpgrade { get; set; }

}