﻿
using System.Net;

namespace RangoAgil.API.EndpointFilters;

public class LogNotFoundResponseFilter : IEndpointFilter
{
    private readonly ILogger<LogNotFoundResponseFilter> _logger;

    public LogNotFoundResponseFilter(ILogger<LogNotFoundResponseFilter> logger) => _logger = logger;

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var result = await next(context);
        var actualResults = (result is INestedHttpResult result1)?result1.Result : (IResult)result;

        if (actualResults is IStatusCodeHttpResult { StatusCode: (int)HttpStatusCode.NotFound }) 
        {
            _logger.LogInformation($"Resource {context.HttpContext.Request.Path} was not found.");
        }

        return result;
    }
}

