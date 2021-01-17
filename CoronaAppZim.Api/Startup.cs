using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Covid19.Client;
using CoronaAppZim.Api.Configuration;
using Microsoft.OpenApi.Models;
using MediatR;

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
            services.AddCors();

            services.AddTransient<ICovid19Client, Covid19Client>();
            services.AddMediatR(typeof(Startup));
            
            services.AddOptions<AWSSNSSettings>()
                .Bind(Configuration.GetSection("AWSSNSSettings"))
                .ValidateDataAnnotations();
                  
            services.Configure<AWSSNSSettings>(Configuration.GetSection("AWSSNSConfig"));
            services.Configure<NewsApiSettings>(Configuration.GetSection("NewsApiConfig"));

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Corona App Zimbabwe",
                    Version = "v1",
                    Description = "Backend for coronazim.info",
                    License = new OpenApiLicense
                    {
                        Name = "MIT",
                        Url = new System.Uri("https://opensource.org/licenses/MIT")
                    },
                });
                config.CustomSchemaIds(x => x.FullName);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //Enable HSTS HTTP Strict Transport Security in production
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "Corona App Zimbabwe");
                config.RoutePrefix = string.Empty;
            });

            app.UseRouting();
           
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
