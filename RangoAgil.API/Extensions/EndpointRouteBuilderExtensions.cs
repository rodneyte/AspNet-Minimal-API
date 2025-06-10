using RangoAgil.API.EndpointFilters;
using RangoAgil.API.EndpointHandlers;

namespace RangoAgil.API.Extensions
{
    public static class EndpointRouteBuilderExtensions
    {
        public static void RegisterRangosEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            var ragosEndpoints = endpointRouteBuilder.MapGroup("/rangos");
            var ragosComIdEndpoints = ragosEndpoints.MapGroup("/{rangoId:int}");

            var rangosComIdEndpointsAndLockFilterEndpoints = endpointRouteBuilder.MapGroup("/rangos/{rangoId:int}")
                 .AddEndpointFilter(new RangoIsLockedFilter(9))
                 .AddEndpointFilter(new RangoIsLockedFilter(7));

            
            ragosComIdEndpoints.MapGet("", RangosHandlers.GetRangoById).WithName("GetRangos");

            ragosEndpoints.MapPost("", RangosHandlers.CreateRangosAsync)
                .AddEndpointFilter<ValidateAnnotationFilter>();

            rangosComIdEndpointsAndLockFilterEndpoints.MapPut("", RangosHandlers.UpdateRangoAsync);


            rangosComIdEndpointsAndLockFilterEndpoints.MapDelete("", RangosHandlers.DeleteRangoAsync)
                .AddEndpointFilter<LogNotFoundResponseFilter>();
               
        }

        public static void RegisterIngredientesEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            var ingradientesEndpoints = endpointRouteBuilder.MapGroup("/rangos/{rangoId:int}/ingredientes");

            ingradientesEndpoints.MapGet("", IngredientesHandlers.GetIngredientesAsync);

            ingradientesEndpoints.MapPost("", () =>
            {
                throw new NotImplementedException();
            });

        }
    }
}
