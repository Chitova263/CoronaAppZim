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
            var allowedOrigins = Configuration.GetSection("CorsPolicy:AllowedOrigins").Value.Split(",") ?? new string[0];
            services.AddCors(x => {
                x.AddPolicy("CoronaAppPolicy", builder => {
                    builder.WithOrigins(allowedOrigins);
                });
            });
            services.AddCovid19Client();
            services.AddMediatR(typeof(Startup));
            
            //enable configuration validation via attributes
            //however validation is only checked once first time at startup
            //becomes buggy if using IOptionsSnapShot<T> which is scoped
            //also see IValidateOptions
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

            //add security headers

            app.UseHttpsRedirection();

            if (env.IsDevelopment())
            {
                app.UseCors(builder => {
                    builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            } 
            else
            {
                app.UseCors("CoronaAppPolicy");
            }

            app.UseSwagger();

            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "Corona App Zimbabwe");
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
