using Microsoft.AspNetCore.Identity;
using RangoAgil.API.EndpointFilters;
using RangoAgil.API.EndpointHandlers;

namespace RangoAgil.API.Extensions
{
    public static class EndpointRouteBuilderExtensions
    {
        public static void RegisterRangosEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGroup("/identity/").MapIdentityApi<IdentityUser>();

            endpointRouteBuilder.MapGet("/pratos/{pratoid:int}", (int pratoid = 0) => $"O prato {pratoid} é delicioso!")
                .WithOpenApi(operation =>
                {
                    operation.Deprecated = true;
                    return operation;
                })
                .WithSummary("Este endpoint esta deprecated e será descontinuado na versão 2 desta API")
                .WithDescription("Por favor utilize a outra rota desta API sendo ela /rangos/{rangoId} para evitar maiores transtornos futuros");

            var rangosEndpoints = endpointRouteBuilder.MapGroup("/rangos")
                .RequireAuthorization();
            var rangosComIdEndpoints = rangosEndpoints.MapGroup("/{rangoId:int}")
                .RequireAuthorization();

            rangosEndpoints.MapGet("", RangosHandlers.GetRangosAsync);
          

            var rangosComIdEndpointsAndLockFilterEndpoints = endpointRouteBuilder.MapGroup("/rangos/{rangoId:int}")
                .RequireAuthorization("RequireAdminFromBrasil")
                .RequireAuthorization()
                 .AddEndpointFilter(new RangoIsLockedFilter(9))
                 .AddEndpointFilter(new RangoIsLockedFilter(7));


            rangosComIdEndpoints.MapGet("", RangosHandlers.GetRangoById).WithName("GetRangos")
                .AllowAnonymous();

            rangosEndpoints.MapPost("", RangosHandlers.CreateRangosAsync)
                .AddEndpointFilter<ValidateAnnotationFilter>();

            rangosComIdEndpointsAndLockFilterEndpoints.MapPut("", RangosHandlers.UpdateRangoAsync);


            rangosComIdEndpointsAndLockFilterEndpoints.MapDelete("", RangosHandlers.DeleteRangoAsync)
                .AddEndpointFilter<LogNotFoundResponseFilter>();
               
        }

        public static void RegisterIngredientesEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            var ingradientesEndpoints = endpointRouteBuilder.MapGroup("/rangos/{rangoId:int}/ingredientes")
                .RequireAuthorization();

            ingradientesEndpoints.MapGet("", IngredientesHandlers.GetIngredientesAsync);

            ingradientesEndpoints.MapPost("", () =>
            {
                throw new NotImplementedException();
            });

        }
    }
}
