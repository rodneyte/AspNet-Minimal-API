﻿using MiniValidation;
using RangoAgil.API.Models;

namespace RangoAgil.API.EndpointFilters;

public class ValidateAnnotationFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var rangoParaCriacaoDTO = context.GetArgument<RangoParaCriacaoDTO>(2);

       if(!MiniValidator.TryValidate(rangoParaCriacaoDTO, out var validationsErros))
        {
            return TypedResults.ValidationProblem(validationsErros);
        }
        return await next(context);
    }
}

