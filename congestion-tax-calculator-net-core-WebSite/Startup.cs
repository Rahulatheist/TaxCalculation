using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using congestion_tax_calculator_net_core_WebSite.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace congestion_tax_calculator_net_core_WebSite
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }
        public IConfigurationRoot Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(action => action.EnableEndpointRouting = false)
                 .AddApplicationPart(typeof(Startup).Assembly);
            services.AddSingleton<IConfiguration>(Configuration);

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton(new HttpClient());
            services.Configure<Myconfig>(Configuration.GetSection("MyConfig"));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc($"v{Configuration["Version"]}", new OpenApiInfo { Title = "Tax Calculator API", Version = "1.0.1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v{Configuration["Version"]}/swagger.json", "Tax calculator API");
                c.RoutePrefix = string.Empty;
            });

            app.UseMvc();
        }
    }
}
