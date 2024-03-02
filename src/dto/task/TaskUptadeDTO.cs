using System.Text.Json.Serialization;

namespace dto;

public class TaskUptadeDTO
{
    public required String Title { get; set; }
    public required String Description { get; set; }
    [JsonIgnore]
    public DateTime DateCreate { get; set; }
    [JsonIgnore]
    public DateTime DateUpgrade { get; set; }

}