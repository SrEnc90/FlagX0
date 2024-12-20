using Microsoft.AspNetCore.Identity;

namespace FlagX0.Web.Data.Entities;

public class FlagEntity
{
    public int Id { get; set; }
    public required string Name { get; set; } = string.Empty;
    public IdentityUser User { get; set; } = null!;
    public required virtual string UserId { get; set; } = string.Empty;
    public required bool Value { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedTimeUtc { get; set; }

}
