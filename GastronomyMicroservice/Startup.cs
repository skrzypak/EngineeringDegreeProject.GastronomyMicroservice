using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GastronomyMicroservice.Core.Fluent;
using GastronomyMicroservice.Core.Interfaces.Services;
using GastronomyMicroservice.Core.Middlewares;
using GastronomyMicroservice.Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace GastronomyMicroservice
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
            services.AddDbContext<MicroserviceContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"), builder => {
                    //builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                });
            });

            services.AddControllers();
            services.AddScoped<ErrorHandlingMiddleware>();

            services.AddAutoMapper(this.GetType().Assembly);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GastronomyMicroservice", Version = "v1" });
            });

            services.AddScoped<IParticipantService, ParticipantService>();
            services.AddScoped<INutritionGroup, NutritionGroupService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GastronomyMicroservice v1"));
            }

            //app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
