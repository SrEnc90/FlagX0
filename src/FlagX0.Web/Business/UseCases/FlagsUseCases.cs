using FlagX0.Web.Business.UseCases.Flags;

namespace FlagX0.Web.Business.UseCases;

public record FlagsUseCases(
    AddFlagUseCase Add,
    GetFlagUseCase GetAll,
    GetPaginatedFlagUseCase GetPaginated,
    GetSingleFlagUseCase Get,
    UpdateFlagUseCase Update,
    DeleteFlagUseCase Delete
) {

}
