namespace ezyGo.Trip.Service.Configuration;

public static class SwaggerConfig
{
    public static IApplicationBuilder AddSwaggerConfiguration(this IApplicationBuilder app, IConfiguration configuration)
    {
        app.UseSwagger(opt => { opt.RouteTemplate = "swagger/{documentName}/swagger.json"; })
            .UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint("/swagger/v1/swagger.json", $"{configuration["Application:Title"]} v1");
                opt.RoutePrefix = "trip";
            });
        return app;
    }
}
