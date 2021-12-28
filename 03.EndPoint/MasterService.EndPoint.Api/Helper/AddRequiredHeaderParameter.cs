using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MasterService.EndPoint.Api.Helper
{
    public class AddRequiredHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters?.Add(new OpenApiParameter
            {
                Name = "X-ValidToken",
                In = ParameterLocation.Header,
                Description = "Master Service Token",
                Required = true,
                Schema = new OpenApiSchema { Type = "string" }
            });
        }
    }
}
