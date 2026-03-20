using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Db;
using DaLang.Lims.Web.Framework.Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DaLang.Lims.Web.Framework.Core.Middlewares;

public class TranscationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public TranscationMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    public async Task InvokeAsync(HttpContext httpContext)
    {
        var endpoint = httpContext.GetEndpoint();

        var feature = httpContext.Features.Get<IExceptionHandlerFeature>();
        // This is now a valid endpoint.
        var ep = feature?.Endpoint?.ToString();

        if (endpoint == null)
        {
            await _next(httpContext);
        }
        else
        {
            if (endpoint.Metadata.Any(m => m.GetType() == typeof(AdminTransactionAttribute)))
            {
                IUnitOfWork unitOfWork = null;
                try
                {
                    unitOfWork = httpContext.RequestServices.GetRequiredService<IUnitOfWork>();
                    unitOfWork.BeginTransaction();
                    await _next(httpContext);
                    unitOfWork.CommitTransaction();
                }
                catch (AppException e)
                {
                    _logger.LogError(e.Message);
                    unitOfWork?.RollbackTransaction();
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    unitOfWork?.RollbackTransaction();
                    throw;
                }
            }
            else
            {
                await _next(httpContext);
            }
        }
    }
}
