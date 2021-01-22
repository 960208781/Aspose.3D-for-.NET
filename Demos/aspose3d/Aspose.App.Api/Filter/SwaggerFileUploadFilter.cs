﻿using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;
using System.Threading.Tasks;

namespace Aspose.App.Api.Filter
{
    public class SwaggerFileUploadFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (!context.ApiDescription.HttpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase) &&
                !context.ApiDescription.HttpMethod.Equals("PUT", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var fileParameters = context.ApiDescription.ActionDescriptor.Parameters.Where(n => n.ParameterType == typeof(IFormFile)).ToList();

            if (fileParameters.Count <= 0)
            {
                return;
            }

            var parameter = operation.Parameters.Single(n => n.Name == fileParameters.First().Name);
            operation.RequestBody = new OpenApiRequestBody()
            {
                Required = parameter.Required,
                Description = parameter.Description,
                Content = new Dictionary<string, OpenApiMediaType>()
                {
                    ["multipart/form-data"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "object",
                            Properties =
                            {
                                ["file"] = new OpenApiSchema() {Type = "file"}
                            }
                        }
                    }
                }
            };
        }
    }
}
