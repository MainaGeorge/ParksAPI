using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ParkyAPI.Data;
using ParkyAPI.Mapper;
using ParkyAPI.Services.IRepositoryService;
using ParkyAPI.Services.RepositoryService;
using System;
using System.IO;
using System.Reflection;

namespace ParkyAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<ApplicationContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<INationalParkRepository, NationalParkRepository>();

            services.AddScoped<ITrailRepository, TrailRepository>();

            services.AddAutoMapper(typeof(Mappings));


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("ParksApi", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Parks API",
                    Description = "info about national parks", 
                    Contact = new OpenApiContact()
                    {
                        Email = "genage08@yahoo.com",
                        Name = "George Maina"
                    }
                });

                var xmlCommentsPath = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var commentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsPath);

                c.IncludeXmlComments(commentsFullPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint("/swagger/ParksApi/swagger.json", "Parks Api");
                opt.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
