namespace FlagX0.Web.DTOs;

public record PaginationResultDto<T>(ICollection<T> Items, int TotalItems, int PageSize, int CurrentPage, string? Search);
