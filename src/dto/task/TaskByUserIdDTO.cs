
namespace dto;

public class TaskByUserIdDTO
{
    public required String Id { get; set; }
    public required String Title { get; set; }
    public required String Description { get; set; }
    public DateTime DateCreate { get; set; }
    public DateTime DateUpgrade { get; set; }
}