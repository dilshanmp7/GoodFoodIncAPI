using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodFoodIncAPI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace GoodFoodIncAPI
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
            
            services.AddSwaggerGen(a =>
            {
                a.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Version = "1.0",
                    Title = "Good Food Inc API",
                    Description = "API Documentation"
                });
            });
            services.AddCors(opt => opt.AddPolicy("Cors", builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
            }));

            services.AddDbContext<GoodFoodIncDBContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("GoodFoodIncDB")));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Formatting = Formatting.None;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(i => { i.SwaggerEndpoint("/swagger/v1/swagger.json", "Good Food Inc API V1"); });
            app.UseMvc();
        }
    }
}
