using FlagX0.Web.Data;
using FlagX0.Web.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static FlagX0.Web.Business.UserInfo.FlagUser;

namespace FlagX0.Web.Business.UseCases.Flags;
// según el libro indica que no es necesario el uso excesivo de interfaces y para su opinión el caso de consultar no necesita interfaces(igual que todo el crud)
// Vamos a utilizar los constructores primarios en vez de hacer el dependency injection por el constructor, acá no se coloca el keword record con el FlagDto,
// el uso de record va a generar propiedades públicas y nosotros la queremos mantener como privadas
public class GetFlagUseCase (ApplicationDbContext _applicationDbContext, IFlagUserDetails _userDetails)
{
    //private readonly ApplicationDbContext _applicationDbContext;
    //private readonly IHttpContextAccessor _contextAccessor;
    //private readonly IFlagUserDetails _userDetails;

    //public GetFlagUseCase(
    //    ApplicationDbContext applicationDbContext,
    //    IHttpContextAccessor contextAccessor,
    //    IFlagUserDetails userDetails)
    //{
    //    _applicationDbContext = applicationDbContext;
    //    _contextAccessor = contextAccessor;
    //    _userDetails = userDetails;
    //}

    public async Task<ICollection<FlagDto>> Execute()
    {
        //var userId = _contextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier).Value; -> Hemos refactorizado la obtención del userId mediante la creación del método UserId de la clase userdetails
        var response = await _applicationDbContext.Flags
            .Where(x => x.UserId == _userDetails.UserId)
            .AsNoTracking()
            .ToListAsync();
        //Vamos a utilizar constructores primario
        //return response.Select(x => new FlagDto()
        //{
        //    Name = x.Name,
        //    IsEnabled = x.Value
        //}).ToList();
        return response.Select(x => new FlagDto()
        {
            Name = x.Name,
            IsEnabled = x.Value
        }).ToList();
    }
}
