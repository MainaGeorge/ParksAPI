using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

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

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Description = "JWT authorization header using the bearer scheme",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            },
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                            Scheme = "oauth2"
                        },

                    new List<string>()
                }
            });

            var commentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var commentsFullPath = Path.Combine(AppContext.BaseDirectory, commentsFile);
            options.IncludeXmlComments(commentsFullPath);
        }
    }
}
