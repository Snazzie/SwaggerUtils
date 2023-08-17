using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Snazzie.SwaggerUtils
{
    /// <summary>
    /// Makes all non nullable properties required by default
    /// Also forces camel case on generated property name
    /// Usage: `options.SchemaFilter<SwaggerRequiredSchemaFilter>();`
    /// </summary>
    public class RequiredIfNotNullableCamelCasedSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var properties = context.Type.GetProperties();
            schema.Required ??= new HashSet<string>();

            foreach (var property in properties)
            {
                var propertyNameInCamelCasing = char.ToLowerInvariant(property.Name[0]) + property.Name[1..];
                var nullabilityContext = new NullabilityInfoContext();
                var nullabilityInfo = nullabilityContext.Create(property);
                if (nullabilityInfo.WriteState is not NullabilityState.Nullable)
                    schema.Required.Add(propertyNameInCamelCasing);
            }
            schema.Nullable = false;
        }
    }

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