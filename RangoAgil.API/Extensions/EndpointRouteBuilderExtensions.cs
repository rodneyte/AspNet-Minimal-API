using RangoAgil.API.EndpointHandlers;

namespace RangoAgil.API.Extensions
{
    public static class EndpointRouteBuilderExtensions
    {
        public static void RegisterRangosEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            var ragosEndpoints = endpointRouteBuilder.MapGroup("/rangos");
            var ragosComIdEndpoints = ragosEndpoints.MapGroup("/{rangoId:int}");


            ragosEndpoints.MapGet("", RangosHandlers.GetRangosAsync);

            ragosComIdEndpoints.MapGet("", RangosHandlers.GetRangoById).WithName("GetRangos");

            ragosEndpoints.MapPost("", RangosHandlers.CreateRangosAsync);

            ragosComIdEndpoints.MapPut("", RangosHandlers.UpdateRangoAsync);

            ragosComIdEndpoints.MapDelete("", RangosHandlers.DeleteRangoAsync);
        }

        public static void RegisterIngredientesEndpoints(this IEndpointRouteBuilder endpointRouteBuilder) 
        {
            var ingradientesEndpoints = endpointRouteBuilder.MapGroup("/rangos/{rangoId:int}/ingredientes");

            ingradientesEndpoints.Map("", IngredientesHandlers.GetRangoIngredientesAsync);

        }
    }
}
