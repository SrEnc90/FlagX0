using FlagX0.Web.Data;
using FlagX0.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using ROP;
using System.Security.Claims;
using static FlagX0.Web.Business.UserInfo.FlagUser;

namespace FlagX0.Web.Business.UseCases.Flags;

//Usando los constructores primarios que se han implementado de net 8
public class AddFlagUseCase (
    IFlagUserDetails _userDetails, /* Estamos haciendo un filtro global en el ApplicationDbContext en el método OnModelCreating*/
    ApplicationDbContext _applicationDbContext)
{
    //private readonly ApplicationDbContext _applicationDbContext;
    //private readonly IHttpContextAccessor _contextAccessor;
    //private readonly IFlagUserDetails _userDetails;

    //public AddFlagUseCase(
    //    ApplicationDbContext context,
    //    IHttpContextAccessor contextAccessor,
    //    IFlagUserDetails userDetails)
    //{
    //    _applicationDbContext = context;
    //    _contextAccessor = contextAccessor;
    //    _userDetails = userDetails;
    //}

    // Hemos cambiado nuestro método Execute por el de abajo usando un librería de netmento para ROP(railway oriented programming)
    //public async Task<bool> Execute(string flagName, bool isActive)
    //{
    //    var userId = _contextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier).Value;
    //    FlagEntity flag = new()
    //    {
    //        Name = flagName,
    //        UserId = userId,
    //        Value = isActive
    //    };
    //    var response = await _applicationDbContext.Flags.AddAsync(flag);
    //    await _applicationDbContext.SaveChangesAsync();

    //    return true;
    //}

    // Usando Railway Oriented Programming
    public async Task<Result<bool>> Execute(string flagName, bool isActive) 
        => await ValidateFlag(flagName)
                .Bind(x => AddFlagToDatabase(x, isActive));
    
    private async Task<Result<string>> ValidateFlag(string flagname)
    {
        bool flagExist = await _applicationDbContext.Flags
                .Where(x => 
                    //x.UserId == _userDetails.UserId &&
                    x.Name.Equals(flagname))
                    .AnyAsync();
        if (flagExist) return Result.Failure<string>("Flag Name Already Exist");
        
        return flagname;
    }

    private async Task<Result<bool>> AddFlagToDatabase(string flagName, bool isActive)
    {
        FlagEntity entity = new()
        {
            Name = flagName,
            UserId = _userDetails.UserId,
            Value = isActive
        };

        await _applicationDbContext.Flags.AddAsync(entity);
        await _applicationDbContext.SaveChangesAsync();

        return true;

    }
}

