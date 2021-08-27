using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ETender.Products.Data;
using ETender.Products.Data.Interfaces;
using ETender.Products.Repositories;
using ETender.Products.Repositories.Interfaces;
using ETender.Products.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace ETender.Products
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

            services.Configure<ProductDatabaseSettings>(Configuration.GetSection(nameof(ProductDatabaseSettings)));

            services.AddSingleton<IProductDatabaseSettings>(x => x.GetRequiredService<IOptions<ProductDatabaseSettings>>().Value);

            services.AddTransient<IProductContext, ProductContext>();
            services.AddTransient<IProductRepository, ProductRepository>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ETender.Products", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ETender.Products v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
