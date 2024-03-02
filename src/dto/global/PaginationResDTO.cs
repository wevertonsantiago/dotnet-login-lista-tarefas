namespace dto;

public class PaginationResDTO<T>
{
    public int TotalPages { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public bool HasNextPage { get; set; }
}