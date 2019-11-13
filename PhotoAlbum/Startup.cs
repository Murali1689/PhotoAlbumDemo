using System.Reflection;
using AutoMapper;
using Lamar;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace PhotoAlbum
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // In production, the Angular files will be served from this directory
            services.AddHttpContextAccessor();

            services.AddMvcCore(setupAction => { setupAction.ReturnHttpNotAcceptable = true; })
                .AddApiExplorer()
                .AddDataAnnotations();

            services.AddSwaggerGen(action => action.SwaggerDoc(Configuration["SwaggerInfo:Version"], new OpenApiInfo
            {
                Title = Configuration["SwaggerInfo:ApplicationName"],
                Version = Configuration["SwaggerInfo:Version"]
            }));
        }

        public void ConfigureContainer(ServiceRegistry services)
        {
            services.Scan(s =>
            {
                s.TheCallingAssembly();
                s.AssembliesFromApplicationBaseDirectory(a => a.GetName().Name.Contains("PhotoAlbum"));
                s.WithDefaultConventions();
            });
            services.AddMediatR(Assembly.GetExecutingAssembly());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(action =>
            {
                action.SwaggerEndpoint($"/swagger/{Configuration["SwaggerInfo:Version"]}/swagger.json", Configuration["SwaggerInfo:ApplicationName"]);
            });
        }
    }
}