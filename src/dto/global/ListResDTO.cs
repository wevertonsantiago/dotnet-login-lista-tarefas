namespace dto;

public class ListResDTO<T>
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage { get; set; }
    public required IEnumerable<T> List { get; set; }
}