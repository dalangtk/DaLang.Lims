using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace DaLang.Lims.Web.Framework.Core.Filters;

/// <summary>
/// Represents the Swagger/Swashbuckle operation filter used to document the implicit API version parameter.
/// </summary>
/// <remarks>This <see cref="IOperationFilter"/> is only required due to bugs in the <see cref="SwaggerGenerator"/>.
/// Once they are fixed and published, this class can be removed.</remarks>
public class SwaggerFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // NOTE: This adds the "Padlock" icon to the endpoint in swagger, 
        //       we can also pass through the names of the policies in the string[]
        //       which will indicate which permission you require.
        operation.Security = new List<OpenApiSecurityRequirement>
        {
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        In = ParameterLocation.Header,
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Authorization"
                        },
                     },
                     new string[] { "global" }
                }
            }
        };
    }
}
