using System.ComponentModel.DataAnnotations;

namespace dto;
public class PageListDTO
{

    const int maxPageSize = 50;
    [Required]
    public int PageNumber { get; set; } = 1;
    private int _pageSize;
    [Required]
    public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {
            _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }

}