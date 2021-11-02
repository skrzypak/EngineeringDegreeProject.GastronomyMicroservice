using System;
using GastronomyMicroservice.Comunication.Consumers;
using GastronomyMicroservice.Core.Fluent;
using GastronomyMicroservice.Core.Interfaces.Services;
using GastronomyMicroservice.Core.Middlewares;
using GastronomyMicroservice.Core.Services;
using GreenPipes;
using MassTransit;
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
                    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                });
            });

            services.AddMassTransit(x =>
            {
                x.AddConsumer<AllergenConsumer>();
                x.AddConsumer<ProductConsumer>();
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
                {
                    config.Host(new Uri("rabbitmq://localhost"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    config.ReceiveEndpoint("msgas.allergen.queue", ep =>
                    {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(r => r.Interval(2, 100));
                        ep.ConfigureConsumer<AllergenConsumer>(provider);
                    });

                    config.ReceiveEndpoint("msgas.product.queue", ep =>
                    {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(r => r.Interval(2, 100));
                        ep.ConfigureConsumer<ProductConsumer>(provider);
                    });

                }));
            });
            services.AddMassTransitHostedService();

            services.AddControllers();
            services.AddScoped<ErrorHandlingMiddleware>();

            services.AddAutoMapper(this.GetType().Assembly);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GastronomyMicroservice", Version = "v1" });
            });

            services.AddScoped<IParticipantService, ParticipantService>();
            services.AddScoped<INutritionGroupService, NutritionGroupService>();
            services.AddScoped<INutritionPlanService, NutritionPlanService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IDishService, DishService>();
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

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
