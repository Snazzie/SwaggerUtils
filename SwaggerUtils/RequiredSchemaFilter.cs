using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Snazzie.SwaggerUtils
{
    public class RequiredSchemaFilter : ISchemaFilter
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
}