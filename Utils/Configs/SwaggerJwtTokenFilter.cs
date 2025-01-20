using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class SwaggerJwtTokenFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var jwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjQiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJjaGFpbnNhd0BleGFtcGxlLmNvbSIsInN0YXR1cyI6IkFjdGl2ZSIsImV4cCI6MTczNjk2NDQ3NSwiaXNzIjoiYXV0aC1zZXJ2aWNlIiwiYXVkIjoiY2xpZW50LWFwcCJ9.xv0qkK_zeYPZnGQh6D50X45OhO4cw6CLqKO0MqYQCZw";

        swaggerDoc.Components.Parameters = new Dictionary<string, OpenApiParameter>
        {
            {"Bearer", new OpenApiParameter
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Schema = new OpenApiSchema {Type = "string"},
                    Required = true,
                    Description = "JWT Token",
                    Example = new OpenApiString($"Bearer {jwtToken}")
                }
            }
        };
    }
}