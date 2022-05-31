using System.Linq;
using Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Middlewares.Authentication;

namespace api
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
            // services.Configure<AppSettings>(
            //    Configuration.GetSection(nameof(AppSettings)));

            // services.AddSingleton<AppSettings>(sp =>
            //     sp.GetRequiredService<IOptions<AppSettings>>().Value);
            var appSettings = Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();

            services.AddSingleton(appSettings);

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.SetIsOriginAllowed(_ => true);
                        builder.AllowAnyMethod();
                        builder.AllowAnyHeader();
                        builder.AllowCredentials();
                    });
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var appSettings = Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();

            app.UseCors();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "api v1"));
            }

            // app.UseHttpsRedirection();

            // app.Use(async (context, next) =>
            // {
            //     // Add Header
            //     context.Response.Headers[appSettings.HeaderSettings.Response.FirstOrDefault().Title] = appSettings.HeaderSettings.Response.FirstOrDefault().Content;

            //     // Call next middleware
            //     await next.Invoke();
            // });

            app.UseMiddleware<AuthenticationMiddleware>();


            app.UseRouting();

            // app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
