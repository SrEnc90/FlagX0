namespace FlagX0.Web.Models;

public class FlagViewModel
{
    public string Name { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
    public string? Error { get; set; } = string.Empty;
}
