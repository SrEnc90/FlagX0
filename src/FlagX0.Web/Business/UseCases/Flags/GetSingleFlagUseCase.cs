using FlagX0.Web.Business.Mappers;
using FlagX0.Web.Data;
using FlagX0.Web.Data.Entities;
using FlagX0.Web.DTOs;
using Microsoft.EntityFrameworkCore;
using ROP;
using static FlagX0.Web.Business.UserInfo.FlagUser;

namespace FlagX0.Web.Business.UseCases.Flags;

public class GetSingleFlagUseCase(
    //IFlagUserDetails _flagUserDetails, -> Estamos haciendo un filtro global en el ApplicationDbContext en el método OnModelCreating
    ApplicationDbContext _applicationDbContext)
{
    public async Task<Result<FlagDto>> Execute(string flagName)
        => await GetDbFrom(flagName)
                .Bind(flag => flag ?? Result.NotFound<FlagEntity>("Flag doesn't exist"))
                .Map(x => x.ToDto());


    private async Task<Result<FlagEntity?>> GetDbFrom(string flagName)
     => await _applicationDbContext.Flags
            //.AsNoTracking()
            //.FirstOrDefaultAsync(a => EF.Functions.Like(a.Name, flagName));
            //.FirstOrDefaultAsync(a => a.Name.Equals(flagName, StringComparison.CurrentCultureIgnoreCase)); 
            // Da error por usar StringComparison en la consulta SQL(No sabe cómo renderizar y transformarlo a consulta) En estos casos sigue las mismas reglas de motor de base de datos:
            //Sql Server, MYSQL, MariaDb son case insensitive
            //Hana y mongodb son case sensitive
            .FirstOrDefaultAsync(a => a.Name.Equals(flagName));
}
