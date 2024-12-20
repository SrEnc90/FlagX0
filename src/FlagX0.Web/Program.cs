using FlagX0.Web.Business.UseCases;
using FlagX0.Web.Business.UseCases.Flags;
using FlagX0.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using static FlagX0.Web.Business.UserInfo.FlagUser;
//using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//API: Añadiendo autorización y autenticación
builder.Services.AddAuthorization();
builder.Services.AddAuthentication()
    .AddCookie("Identity.Bearer");

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

//Vamos a evitar el uso excesivo de inyección de interfaces para hacer el CRUD
//builder.Services.AddScoped<IAddFlagUseCase, AddFlagUseCase>();

builder.Services.AddScoped<FlagsUseCases>();
builder.Services.AddScoped<GetFlagUseCase>();
builder.Services.AddScoped<GetPaginatedFlagUseCase>();
builder.Services.AddScoped<GetSingleFlagUseCase>();
builder.Services.AddScoped<AddFlagUseCase>();
builder.Services.AddScoped<UpdateFlagUseCase>();
builder.Services.AddScoped<DeleteFlagUseCase>();
builder.Services.AddScoped<IFlagUserDetails, FlagUserDetails>();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("token", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        In = ParameterLocation.Header,
        Name = HeaderNames.Authorization,
        Scheme = "bearer"
    });
});

var app = builder.Build();


// Ver si es peferible colocar: using var scope = app.Services.CreateScope(); o usar using (var scope ...) -> con paréntesis
using (var scope = app.Services.CreateScope())
{
    ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();

    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FlagX0.Web v1"));
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.MapGroup("/account").MapIdentityApi<IdentityUser>(); // Al ejecutar el proyecto en el swagger te genera otro controlador para el account(tiene los enpoints de register, login, refresh, confirmEmail, forgotPassword, etc.) Estos endpoints nos los da la librería de Identity

app.Run();
