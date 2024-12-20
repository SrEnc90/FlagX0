using FlagX0.Web.DTOs;

namespace FlagX0.Web.Models;

public class FlagIndexViewModel
{
    public ICollection<FlagDto> Flags { get; set; } = null!;
}
