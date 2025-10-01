using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using CaseStudy.WebApi.Configure;
using CaseStudy.WebApi.Data;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add OpenAPI with versions
OpenApiConfigure openApiConfigure = new(builder.Configuration);
builder.Services.AddOpenApi("v1", options => openApiConfigure.CreateOpenApiInfo(ref options, "v1"));
builder.Services.AddOpenApi("v2", options => openApiConfigure.CreateOpenApiInfo(ref options, "v2", "This version of the API adds support for pagination when retrieving the list of all products."));

// Add API versioning to the project
builder.Services.AddApiVersioning(option =>
{
    option.AssumeDefaultVersionWhenUnspecified = true;
    option.DefaultApiVersion = new ApiVersion(1);
    option.ReportApiVersions = true;
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Register the DbContext with a connection string from appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        // Build a swagger endpoint for each discovered API version
        IApiVersionDescriptionProvider provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/openapi/{description.GroupName}.json", description.GroupName.ToLowerInvariant());
        }
    });

}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Add global exception handler
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        Exception? exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        if (exception != null)
        {
            var result = new
            {
                error = exception.Message,
                stackTrace = exception.StackTrace
            };
            await context.Response.WriteAsJsonAsync(result);
        }
    });
});

app.Run();
