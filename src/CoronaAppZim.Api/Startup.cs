using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Covid19.Client;
using CoronaAppZim.Api.Config;
using Microsoft.OpenApi.Models;
using MediatR;
using System.Reflection;

namespace CoronaAppZim.Api
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
            services.AddControllers();
            services.AddHttpClient();
            services.AddCovid19Client();
            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);
            services.Configure<AWSSNSSettings>(Configuration.GetSection("AWSSNSConfig"));
            services.Configure<NewsApiSettings>(Configuration.GetSection("AWSSNSConfig"));
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Corona App Zimbabwe API",
                    Version = "v1",
                    Description = "Backend for coronazim.info",
                    License = new OpenApiLicense
                    {
                        Name = "MIT",
                        Url = new System.Uri("https://opensource.org/licenses/MIT")
                    },
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseSwagger();

            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "Corona App Zimbabwe API");
                config.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
