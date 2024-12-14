using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace GameRaitingAPI.Utility
{
    public static class SwaggerExtensions
    {
        public static TBuilder Filter<TBuilder>(this TBuilder builder)
            where TBuilder : IEndpointConventionBuilder
        {
            return builder.WithOpenApi(options =>
            {
                options.Parameters.Add(new OpenApiParameter
                {
                    Name = "Page",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "integer",
                        Default = new OpenApiInteger(1)
                    }
                });

                options.Parameters.Add(new OpenApiParameter
                {
                    Name = "RecordsPerPage",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "integer",
                        Default = new OpenApiInteger(10)
                    }
                });

                options.Parameters.Add(new OpenApiParameter
                {
                    Name = "Name",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                    }
                });

                options.Parameters.Add(new OpenApiParameter
                {
                    Name = "GanreId",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "integer",
                    }
                });

                options.Parameters.Add(new OpenApiParameter
                {
                    Name = "Field",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                        Enum = new List<IOpenApiAny> { new OpenApiString("Name"),
                            new OpenApiString("ReleaseDate")}
                    }
                });

                options.Parameters.Add(new OpenApiParameter
                {
                    Name = "UpcomingGames",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "boolean",
                        Default = new OpenApiBoolean(false)
                    }
                });

                options.Parameters.Add(new OpenApiParameter
                {
                    Name = "OrderByAscending",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "boolean",
                        Default = new OpenApiBoolean(true)
                    }
                });

                return options;
            });
        }

        public static TBuilder Pagination<TBuilder>(this TBuilder builder)
            where TBuilder : IEndpointConventionBuilder
        {
            return builder.WithOpenApi(options =>
            {
                options.Parameters.Add(new OpenApiParameter
                {
                    Name = "Page",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "integer"
                    }
                });

                options.Parameters.Add(new OpenApiParameter
                {
                    Name = "RecordsPerPage",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "integer"
                    }
                });

                return options;
            });
        }
    }
}
