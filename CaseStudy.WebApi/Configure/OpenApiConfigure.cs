using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace CaseStudy.WebApi.Configure
{
    /// <summary>
    /// Configure class for OpenAPI options
    /// </summary>
    public class OpenApiConfigure
    {
        private readonly IConfiguration configuration;

        public OpenApiConfigure(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Create a default OpenAPI document with title, description and contact information. 
        /// Informations are read from appsettings.json
        /// </summary>
        /// <param name="options">Working <see cref="OpenApiOptions"/></param>
        /// <param name="version">Version of API</param>
        /// <param name="versionFeatures">New features in this version of API</param>
        public void CreateOpenApiInfo(ref OpenApiOptions options, string version, string versionFeatures = "")
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Info = new OpenApiInfo
                {
                    Title = $"{configuration["OpenApiInfo:Title"]} {version}",
                    Description = $"{configuration["OpenApiInfo:Description"]} {versionFeatures}",
                    Contact = new OpenApiContact
                    {
                        Name = configuration["OpenApiInfo:Contact:Name"],
                        Email = configuration["OpenApiInfo:Contact:Email"],
                        Url = new Uri(configuration["OpenApiInfo:Contact:Url"]!)
                    }
                };
                return Task.CompletedTask;
            });
        }
    }
}
