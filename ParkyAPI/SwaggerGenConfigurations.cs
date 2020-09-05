using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ParkyAPI
{
    public class SwaggerGenConfigurations : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _versionDescriptionProvider;

        public SwaggerGenConfigurations(IApiVersionDescriptionProvider versionDescriptionProvider)
        {
            _versionDescriptionProvider = versionDescriptionProvider;
        }
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _versionDescriptionProvider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    description.GroupName, new OpenApiInfo
                        {
                        Version = description.ApiVersion.ToString(),
                        Title = $"Parks Api {description.ApiVersion}"
                        }
                );
            }

            var commentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var commentsFullPath = Path.Combine(AppContext.BaseDirectory, commentsFile);
            options.IncludeXmlComments(commentsFullPath);
        }
    }
}
