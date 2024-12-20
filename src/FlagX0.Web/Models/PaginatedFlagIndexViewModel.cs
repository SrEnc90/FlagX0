using FlagX0.Web.DTOs;

namespace FlagX0.Web.Models;

public class PaginatedFlagIndexViewModel
{
    public PaginationResultDto<FlagDto> Pagination { get; set; }
    public ICollection<int> SelectOptions { get; set; } = [5, 10, 15];
}
