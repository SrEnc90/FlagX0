using FlagX0.Web.DTOs;

namespace FlagX0.Web.Models;

public class SingleFlagViewModel
{
    public string? Message { get; set; } = string.Empty;
    public FlagDto Flag { get; set; } = null!;
}
