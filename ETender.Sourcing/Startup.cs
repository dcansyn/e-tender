using ETender.Sourcing.Data;
using ETender.Sourcing.Data.Interfaces;
using ETender.Sourcing.Hubs;
using ETender.Sourcing.Repositories;
using ETender.Sourcing.Repositories.Interfaces;
using ETender.Sourcing.Settings;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Producer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETender.Sourcing
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
            services.Configure<SourcingDatabaseSettings>(Configuration.GetSection(nameof(SourcingDatabaseSettings)));
            services.AddSingleton<ISourcingDatabaseSettings>(x => x.GetRequiredService<IOptions<SourcingDatabaseSettings>>().Value);

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ETender.Sourcing", Version = "v1" });
            });

            services.AddTransient<ISourcingContext, SourcingContext>();
            services.AddTransient<IAuctionRepository, AuctionRepository>();
            services.AddTransient<IBidRepository, BidRepository>();

            services.AddAutoMapper(typeof(Startup));

            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                var factory = new ConnectionFactory()
                {
                    HostName = Configuration["EventBus:HostName"]
                };

                if (!string.IsNullOrWhiteSpace(Configuration["EventBus:UserName"]))
                {
                    factory.UserName = Configuration["EventBus:UserName"];
                }

                if (!string.IsNullOrWhiteSpace(Configuration["EventBus:Password"]))
                {
                    factory.UserName = Configuration["EventBus:Password"];
                }

                var retryCount = 5;
                if (!string.IsNullOrWhiteSpace(Configuration["EventBus:RetryCount"]))
                {
                    retryCount = int.Parse(Configuration["EventBus:RetryCount"]);
                }

                return new DefaultRabbitMQPersistentConnection(factory, retryCount, logger);
            });

            services.AddSingleton<EventBusRabbitMQProducer>();

            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .WithOrigins("https://localhost:44398");
            }));

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ETender.Sourcing v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors("CorsPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<AuctionHub>("/auctionhub");
                endpoints.MapControllers();
            });
        }
    }
}
