using FlagX0.Web.Business.UseCases.Flags;
using FlagX0.Web.Data;
using FlagX0.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FlagX0.Web.Business.UserInfo.FlagUser;

namespace FlagX0.UnitTests.Web.Business.UseCases.Flags;


/// <summary>
/// Pruebas unitarias para el caso de uso AddFlagUseCase
/// Para lo cual hemos creado un proyecto de XUnit
/// </summary>
public class AddFlagUseCaseTest
{
    [Fact]
    public async Task WhenFlagNameAlreadyExist_ThenError()
    {
        //arrange -> Preparar
        IFlagUserDetails flagUserDetails = new FlagUserDetailsStud();

        ApplicationDbContext inMemoryDb = GetInMemoryDbContext(flagUserDetails);

        FlagEntity currentFlag = new()
        {
            Name = "Flag1",
            UserId = flagUserDetails.UserId,
            Value = true
        };

        await inMemoryDb.Flags.AddAsync(currentFlag);
        await inMemoryDb.SaveChangesAsync();


        //act -> Ejecutar
        AddFlagUseCase addFlagUseCase = new(flagUserDetails, inMemoryDb);
        var result = await addFlagUseCase.Execute(currentFlag.Name, true);


        //assert -> Validar
        Assert.False(result.Success);
        Assert.Equal("Flag Name Already Exist", result.Errors.First().Message);
    }

    [Fact]
    public async Task WhenFlagDoesNotExist_ThenInsertedOnDb()
    {
        //arrange -> Preparar
        IFlagUserDetails flagUserDetails = new FlagUserDetailsStud();
        ApplicationDbContext inMemoryDb = GetInMemoryDbContext(flagUserDetails);
        FlagEntity currentFlag = new()
        {
            Name = "Flag1",
            UserId = flagUserDetails.UserId,
            Value = true
        };
        //act -> Ejecutar
        AddFlagUseCase addFlagUseCase = new(flagUserDetails, inMemoryDb);
        var result = await addFlagUseCase.Execute(currentFlag.Name, true);
        //assert -> Validar
        Assert.True(result.Success);
        Assert.True(result.Value);
        Assert.Empty(result.Errors);
        FlagEntity? flagInserted = await inMemoryDb.Flags.FirstOrDefaultAsync();
        Assert.NotNull(flagInserted);
        Assert.Equal(currentFlag.Name, flagInserted.Name);
        Assert.Equal(currentFlag.UserId, flagInserted.UserId);
        Assert.Equal(currentFlag.Value, flagInserted.Value);
    }

    private ApplicationDbContext GetInMemoryDbContext(IFlagUserDetails flagUserDetails)
    {
        // Para utilizar una base de datos en memoria necesitamos instalar el paquete nuget de Microsoft.EntityFrameworkCore.InMemory
        DbContextOptions<ApplicationDbContext> databaseOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("flagx0Db")
            .Options;

        return new ApplicationDbContext(databaseOptions, flagUserDetails); //Esta bbdd es en memoria por lo que si queremos consultar registros primero debemos insertarlos

    }
}

public class FlagUserDetailsStud : IFlagUserDetails
{
    public string UserId => "1";
}
