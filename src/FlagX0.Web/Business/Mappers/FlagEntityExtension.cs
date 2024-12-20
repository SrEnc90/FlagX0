using FlagX0.Web.Data.Entities;
using FlagX0.Web.DTOs;

namespace FlagX0.Web.Business.Mappers;

public static class FlagEntityExtension
{
    public static FlagDto ToDto(this FlagEntity entity)
        => new FlagDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            IsEnabled = entity.Value
        };

    public static List<FlagDto> ToDto(this List<FlagEntity> entities)
        => entities.Select(ToDto).ToList();
}
