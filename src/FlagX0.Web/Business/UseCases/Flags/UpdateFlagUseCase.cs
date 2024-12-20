using FlagX0.Web.Business.Mappers;
using FlagX0.Web.Data;
using FlagX0.Web.Data.Entities;
using FlagX0.Web.DTOs;
using Microsoft.EntityFrameworkCore;
using ROP;
using static FlagX0.Web.Business.UserInfo.FlagUser;

namespace FlagX0.Web.Business.UseCases.Flags;

public class UpdateFlagUseCase(
    //IFlagUserDetails _userDetails, -> Estamos haciendo un filtro global en el ApplicationDbContext en el método OnModelCreating
    ApplicationDbContext _applicationDbContext)
{
    public async Task<Result<FlagDto>> Execute(FlagDto flagDto)
        => await VerifyIsTheOnlyOneWithThatName(flagDto)
                .Bind(x => GetFromDb(x.Id))
                .Bind(x => Update(x, flagDto))
                .Map(x => x.ToDto());

    private async Task<Result<FlagDto>> VerifyIsTheOnlyOneWithThatName(FlagDto flagDto)
    {
        bool alreadyExist = await _applicationDbContext.Flags
                        .AnyAsync(a => 
                            //a.UserId == _userDetails.UserId &&
                            //&& EF.Functions.Like(a.Name, flagDto.Name)

                            //&& a.Name.Equals(flagDto.Name, StringComparison.CurrentCultureIgnoreCase)
                            // Da error por usar StringComparison en la consulta SQL(No sabe cómo renderizar y transformarlo a consulta) En estos casos sigue las mismas reglas de motor de base de datos:
                            //Sql Server, MYSQL, MariaDb son case insensitive
                            //Hana y mongodb son case sensitive

                            a.Name.Equals(flagDto.Name)
                            && a.Id != flagDto.Id);
        if(alreadyExist)
        {
            return Result.Failure<FlagDto>("Flag with the same name already exist");
        }

        return flagDto;
    }

    private async Task<Result<FlagEntity?>> GetFromDb(int id) =>
            await _applicationDbContext.Flags
                    .FirstOrDefaultAsync(x => 
                        //x.UserId == _userDetails.UserId && 
                        x.Id == id);

    private async Task<Result<FlagEntity>> Update(FlagEntity? flagEntity, FlagDto flagDto)
    {
        flagEntity!.Value = flagDto.IsEnabled;
        flagEntity.Name = flagDto.Name;
        await _applicationDbContext.SaveChangesAsync();
        return flagEntity;
    }
           
    
}
