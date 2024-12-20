using FlagX0.Web.Business.UseCases;
using FlagX0.Web.Business.UseCases.Flags;
using FlagX0.Web.DTOs;
using FlagX0.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ROP;
using System.ComponentModel.DataAnnotations;

namespace FlagX0.Web.Controllers;

[Authorize]
[Route("[controller]")]
[ApiExplorerSettings(IgnoreApi = true)]
public class FlagsController (
    //Los hemos agrupado en una archivo llamado FlagsUseCases.cs
    //GetFlagUseCase _getFlagUseCase, 
    //GetPaginatedFlagUseCase _getPaginatedFlagUseCase,
    //GetSingleFlagUseCase _getSingleFlagUseCase,
    //AddFlagUseCase _addFlagUseCase, 
    //UpdateFlagUseCase _updateFlagUseCase,
    //DeleteFlagUseCase _deleteFlagUseCase
    FlagsUseCases _flags
    ) : Controller //Usando los constructores primarios
{

    //private readonly GetFlagUseCase _getFlagUseCase;
    //private readonly AddFlagUseCase _addFlagUseCase;

    //public FlagsController(
    //    AddFlagUseCase addFlagUseCase,
    //    GetFlagUseCase getFlagUseCase)
    //{
    //    _addFlagUseCase = addFlagUseCase;
    //    _getFlagUseCase = getFlagUseCase;
    //}

    //Sin Paginación
    [HttpGet("Index")]
    public async Task<IActionResult> Index()
    {
        //var listFlags = await _getFlagUseCase.Execute();
        var listFlags = await _flags.GetAll.Execute();

        return View(new FlagIndexViewModel()
        {
            Flags = listFlags
        });
    }

    //Con Paginación
    [HttpGet("")]
    [HttpGet("IndexPaginated")]
    [HttpGet("{page:int}")]
    public async Task<IActionResult> PaginatedIndex(
        string? search, 
        /*[Range(1, int.MaxValue, ErrorMessage = "page must be > 1")] */int page = 1, //La validación la podemos hacer desde el controlador usando el modelState, pero no es recomendable, la hice dentro del usecase
        int size = 5
     )
    {
        //Ya no vamos hacer la validación de la página desde el controlador, sino desde el usecase
        //if(!ModelState.IsValid)
        //{
        //    page = 1;
        //}

        //var listFlags = await _getPaginatedFlagUseCase.Execute(search, page, size).ThrowAsync();
        var listFlags = await _flags.GetPaginated
                            .Execute(search, page, size)
                            .ThrowAsync();

        return View("PagedFlags", new PaginatedFlagIndexViewModel()
        {
            Pagination = listFlags
        });

    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        return View(new FlagViewModel());
    }

    //Hemos cambiado este método para usar ROP(Railway Oriented Programming -> para lo cuál no usamos la inyección de la interface IAddFlagUse, sino desde la misma clase)
    //[HttpPost("create")]
    //public async Task<IActionResult> AddFlagToDatabase(FlagViewModel request)
    //{
    //    bool isCreated = await _addFlagUseCase.Execute(request.Name, request.IsEnabled);
    //    return RedirectToAction("Index");
    //}

    /// <summary>
    /// endpoint que agrega un nuevo registro usando ROP(Railway Oriented Programming) validando si existe ese registro y viendo si hay errores
    /// </summary>
    /// <param name="request">Módelo que se mapea con la vista (es diferente a un dto)</param>
    /// <returns>si es successfully la vista index, sino la misma vista de AddFlagToDatabase con un mensaje de error</returns>
    /// 
    [HttpPost("create")]
    public async Task<IActionResult> AddFlagToDatabase(FlagViewModel request)
    {
        //Result<bool> isCreated = await _addFlagUseCase.Execute(request.Name, request.IsEnabled);
        Result<bool> isCreated = await _flags.Add.Execute(request.Name, request.IsEnabled);

        if(isCreated.Success)
        {
            return RedirectToAction("Index");
        }

        return View("create", new FlagViewModel()
        {
            Name = request.Name,
            IsEnabled = request.IsEnabled,
            Error = isCreated.Errors.FirstOrDefault()?.Message
        });
    }

    [HttpGet("{flagName}")]
    public async Task<IActionResult> GetSingle(string flagName, string? message)
    {
        //var singleFlag = await _getSingleFlagUseCase.Execute(flagName);
        var singleFlag = await _flags.Get.Execute(flagName);

        return View("SingleFlag", new SingleFlagViewModel()
        {
            Flag = singleFlag.Value,
            Message = message
        });
    }

    [HttpPost("{flagName}")]
    public async Task<IActionResult> Update(FlagDto flag)
    {
        //var singleFlag = await _updateFlagUseCase.Execute(flag);
        var singleFlag = await _flags.Update.Execute(flag);

        return View("SingleFlag", new SingleFlagViewModel
        {
            Flag = singleFlag.Value,
            Message = singleFlag.Success ? "Updated correctly" : singleFlag.Errors.First().Message
        });
    }

    [HttpGet("delete/{flagName}")]
    public async Task<IActionResult> Delete(string flagName)
    {
        //var isDeleted = await _deleteFlagUseCase.Execute(flagName);
        var isDeleted = await _flags.Delete.Execute(flagName);
        if (!isDeleted.Success)
        {
            return RedirectToAction("GetSingle", new
            {
                flagName = flagName,
                message = isDeleted.Errors.FirstOrDefault()?.Message
            });
        }

        return RedirectToAction("Index");
    }

}
