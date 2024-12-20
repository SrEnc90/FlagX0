using FlagX0.Web.Business.UseCases.Flags;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ROP;
using ROP.APIExtensions;

namespace FlagX0.Web.Controllers.API;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class FlagsController(GetSingleFlagUseCase _getSingleFlagUseCase) : ControllerBase
{
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status404NotFound)]
    [HttpGet("{flagName}")]
    public async Task<IActionResult> GetSingleFlag([FromRoute] string flagName)
        => await _getSingleFlagUseCase.Execute(flagName)
                .Map(flag => flag.IsEnabled)
                .ToActionResult();



}
