using FlagX0.Web.Data;
using FlagX0.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using ROP;
using static FlagX0.Web.Business.UserInfo.FlagUser;

namespace FlagX0.Web.Business.UseCases.Flags;

public class DeleteFlagUseCase(
    //IFlagUserDetails _flagUserDetails, -> Estamos haciendo un filtro global en el ApplicationDbContext en el método OnModelCreating
    ApplicationDbContext _applicationDbContext)
{

    public async Task<Result<bool>> Execute(string flagName)
        => await GetEntity(flagName)
            .Bind(flag => flag ?? Result.NotFound<FlagEntity>("Flag doesn't exist"))
            .Bind(DeleteEntity);
    
    private async Task<Result<FlagEntity?>> GetEntity(string flagName)
        =>  await _applicationDbContext.Flags
                .FirstOrDefaultAsync(x => 
                    x.Name.Equals(flagName)
                    //&& EF.Functions.Like(flagName, x.Name)
                    //&& x.UserId == _flagUserDetails.UserId
                );


    private async Task<Result<bool>> DeleteEntity(FlagEntity flagEntity)
    {
        //Soft Deleted
        flagEntity.IsDeleted = true;
        flagEntity.DeletedTimeUtc = DateTime.UtcNow;
        await _applicationDbContext.SaveChangesAsync();
        return true;

        //Hard Deleted
        //_applicationDbContext.Flags.Remove(flagEntity);
        //await _applicationDbContext.SaveChangesAsync();
        //return true;
    } 
}
