using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PostalTracking.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc;

namespace PostalTracking.API
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
            services.AddMvc();
            services.AddDbContext<PostalTrackingContext>(options => options.UseSqlServer(Configuration["dbConnString"]));

            // Dodavanje swaggera za servis
            //services.AddSwaggerGen(options =>
            //{
            //    options.SwaggerDoc("v1.0", new Info { Title = "Postal API", Version = "v1.0" });   // Dodavanje swagger dokumenta za verziju 1.0
            //    options.SwaggerDoc("v1.1", new Info { Title = "Postal API", Version = "v1.1" });   // Dodavanje swagger dokumenta za verziju 1.1

            //    // Ovime se implementira logika za odlucivanje u koji dokument ce ici koja verzija servisa
            //    options.DocInclusionPredicate((docName, apiDesc) =>
            //    {
            //        var versions = apiDesc.ControllerAttributes()
            //                            .OfType<ApiVersionAttribute>()
            //                            .SelectMany(attr => attr.Versions);

            //        return versions.Any(v => $"v{v.ToString()}" == docName);
            //    });

            //    options.IncludeXmlComments(GetXmlCommentsPath());
            //});

            // Register the Swagger generator, defining one or more Swagger documents  
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Postal API", Version = "v1" });
                c.IncludeXmlComments(GetXmlCommentsPath());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseMvc();

            // Enable middleware to serve generated Swagger as a JSON endpoint.  
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.  
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                          name: "default",
                          template: "{controller=Todo}/{action=Index}/{id?}");
            });
        }

        private string GetXmlCommentsPath()
        {
            var app = PlatformServices.Default.Application;
            return System.IO.Path.Combine(app.ApplicationBasePath, "PostalTracking.API.xml");
        }
    }
}
