using FlagX0.Web.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static FlagX0.Web.Business.UserInfo.FlagUser;

namespace FlagX0.Web.Data;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IFlagUserDetails flagUserDetails
    ) : IdentityDbContext(options)
{
    public DbSet<FlagEntity> Flags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Modificando ApplicationDbContext para que toda consulta que tenga el FlagEntity, incluya el filtro(y así no tener que actualizar en cada controller o dónde sea que queremos que no se muestren los eliminados)
        modelBuilder.Entity<FlagEntity>()
            .HasQueryFilter(x => !x.IsDeleted
                    && x.UserId == flagUserDetails.UserId);

        base.OnModelCreating(modelBuilder);
    }

}
