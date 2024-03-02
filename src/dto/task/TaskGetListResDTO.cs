
using System.Text.Json.Serialization;

namespace dto;

public class TaskListResDTO<T> : PaginationResDTO<T>
{
    [JsonPropertyOrder(order: 1)]
    public required IEnumerable<T> List { get; set; }
}