using FlagX0.Web.Business.Mappers;
using FlagX0.Web.Data;
using FlagX0.Web.Data.Entities;
using FlagX0.Web.DTOs;
using Microsoft.EntityFrameworkCore;
using ROP;

namespace FlagX0.Web.Business.UseCases.Flags;

public class GetPaginatedFlagUseCase(
    //IFlagUserDetails _flagUserDetails, -> Estamos haciendo un filtro global en el ApplicationDbContext en el método OnModelCreating
    ApplicationDbContext _applicationDbContext)
{
    public async Task<Result<PaginationResultDto<FlagDto>>> Execute(string? search, int page, int size)
        // con la validación de los parámetros de entrada
        => await ValidatePage(page)
                .Fallback(_ =>
                {
                    page = 1;
                    return Result.Unit;

                })
                .Bind(_ => ValidatePageSize(size)
                            .Fallback(_ =>
                            {
                                size = 5;
                                return Result.Unit;
                            })
                ).Async()
                .Bind(x => GetFromDb(search, page, size)
                .Map(x => x.ToDto())
                .Combine(x => TotalElements(search))
                .Map(x => new PaginationResultDto<FlagDto>(x.Item1, x.Item2, size, page, search)));
    //=> await GetFromDb(search, page, size)
    //        .Map(x => x.ToDto())
    //        .Combine(x => TotalElements(search))
    //        .Map(x => new PaginationResultDto<FlagDto>(x.Item1, x.Item2, size, page, search));

    private Result<Unit> ValidatePage(int page)
    {
        if (page < 1) return Result.Failure<Unit>("Page must be > 1");
        return Result.Unit;
    }

    private Result<Unit> ValidatePageSize(int pageSize)
    {
        //int[] allowValues = [5, 10, 15]; // sintaxis con []
        int[] allowValues = { 5, 10, 15 }; // sintaxis con {}
        if (!allowValues.Contains(pageSize))
            return Result.Failure<Unit>("Page size must be 5, 10 or 15");
        return Result.Unit;
    }

    private async Task<Result<List<FlagEntity>>> GetFromDb(string? search, int page, int size)
    {
        var query = _applicationDbContext.Flags
            //.Where(x => x.UserId == _flagUserDetails.UserId)
            .Skip(size * (page - 1))
            .Take(size);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query.Where(x => x.Name.Equals(search));
        }
        return await query.ToListAsync(); // colocar recién el ToListAsync cuándo vamos a retornar, esto nos permite filtrar aún más si cabe la búsqueda. Esta consulta no va hacer ejecutada hasta que ejecutemos el ToListAsync()
    }

    private async Task<Result<int>> TotalElements(string? search)
    {
        var query = _applicationDbContext.Flags;
            //.Where(x => x.UserId == _flagUserDetails.UserId);

        if (!string.IsNullOrWhiteSpace(search))
        {
            //query = query.Where(x => x.Name.Equals(search));
            return await _applicationDbContext.Flags
                .Where(x => x.Name.Equals(search))
                .CountAsync();
        }

        return await query.CountAsync();
    }
}
