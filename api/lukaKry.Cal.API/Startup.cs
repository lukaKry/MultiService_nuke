using lukaKry.Calc.API.DataAccess;
using lukaKry.Calc.API.Services;
using lukaKry.Calc.Library.Interfaces;
using lukaKry.Calc.Library.Logic;
using lukaKry.Calc.Library.Logic.CalculationsBuilders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lukaKry.Calc.API
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
            bool isInDockerContainer = (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true");
            string connectionString = isInDockerContainer ? "AppDbContext" : "DefaultConnection";

            // local db
            // services.AddDbContext<CalculationDataContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));

            // dockerized db
            services.AddDbContext<CalculationDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString(connectionString)));

            services.AddSingleton<CalculationsFactoryProvider>();
            services.AddScoped<CalculatorService>();
            services.AddSingleton<ICalculationBuilder, EquationCalculationBuilder>();
            services.AddScoped<IEquationsRepository, DatabaseArchiver>();

            services.AddCors(options => options.AddDefaultPolicy(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "lukaKry.Calc.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors();
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "lukaKry.Calc.API v1"));
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
