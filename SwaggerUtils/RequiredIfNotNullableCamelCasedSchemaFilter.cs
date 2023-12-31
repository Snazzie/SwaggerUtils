﻿using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Snazzie.SwaggerUtils
{
    /// <summary>
    /// Makes all non nullable properties required by default
    /// Usage: `options.SchemaFilter<SwaggerRequiredSchemaFilter>();`
    /// </summary>
    public class RequiredIfNotNullableSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var properties = context.Type.GetProperties();
            schema.Required ??= new HashSet<string>();

            foreach (var property in properties)
            {
                var nullabilityContext = new NullabilityInfoContext();
                var nullabilityInfo = nullabilityContext.Create(property);
                if (nullabilityInfo.WriteState is not NullabilityState.Nullable)
                    schema.Required.Add(property.Name);
            }
            schema.Nullable = false;
        }
    }
}