using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RangoAgil.API.DbContexts;
using RangoAgil.API.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<RangoDbContext>(
    o => o.UseSqlite(builder.Configuration["ConnectionStrings:RangoDbConStr"])
    );

builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<RangoDbContext>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddProblemDetails();

builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("RequireAdminFromBrasil", policy =>
    policy.RequireRole("admin")
    .RequireClaim("country","Brazil"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option => 
{
    option.AddSecurityDefinition("TakenAuthRango",
        new()
        {
            Name= "Authorization",
            Description = "Token baseado em Autenticação e Autorização",
            Type = SecuritySchemeType.Http,
            Scheme="Bearer",
            In=ParameterLocation.Header
        }
        );
    option.AddSecurityRequirement(new()
        {
            {
             new()
             {
                 Reference = new OpenApiReference
                 {
                     Type = ReferenceType.SecurityScheme,
                     Id= "TakenAuthRango"
                 }
             },
             new List<string>()
            }
        }
        );


});


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
    //app.UseExceptionHandler(ConfigurationBinder =>
    //{
    //    ConfigurationBinder.Run(
    //        async context =>
    //        {
    //            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
    //            context.Response.ContentType = "text/html";
    //            await context.Response.WriteAsync("uma exeção inesperada aconteceu.");
    //        });
    //}
    //);
}

app.UseHttpsRedirection();

//app.UseAuthentication();
//app.UseAuthorization();

if (app.Environment.IsDevelopment()) {  
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.RegisterRangosEndpoints();
app.RegisterIngredientesEndpoints();

app.Run();
