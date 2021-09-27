using FluentValidation.AspNetCore;
using MealAnalyzer.Core.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Zafar.MealAnalyzer.Core.Abstractions;
using Zafar.MealAnalyzer.Core.Helpers;
using Zafar.MealAnalyzer.Core.Implementations;
using Zafar.MealAnalyzer.Core.Models;
using Zafar.MealAnalyzer.Core.QueryHandlers;

namespace Zafar.MealAnalyzer.WebApi
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
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            });
            services.AddDistributedMemoryCache();
            services.AddSwaggerGenNewtonsoftSupport();

            services.AddTransient<ActiveUserAnalyzer>();
            services.AddTransient<SuperActiveUserAnalyzer>();
            services.AddTransient<BoredUserAnalyzer>();

            services.AddTransient<UserAnalyzeServiceResolver>(provider => userType =>
            {
                return userType switch
                {
                    UserType.ACTIVE => provider.GetService<ActiveUserAnalyzer>(),
                    UserType.SUPERACTIVE => provider.GetService<SuperActiveUserAnalyzer>(),
                    UserType.BORED => provider.GetService<BoredUserAnalyzer>(),
                    _ => throw new KeyNotFoundException(),
                };
            });

            services.AddScoped<IDataReader, JsonDataReader>();
            services.AddScoped<ICacheHelper, CacheHelper>();
            services.AddMediatR(typeof(MealAnalyzeQueryHandler).GetTypeInfo().Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));

            services.AddControllers()
                    .AddNewtonsoftJson(o =>
            {
                o.SerializerSettings.Converters.Add(new StringEnumConverter());
            })
             .AddFluentValidation(v => v.RegisterValidatorsFromAssemblyContaining<Startup>()); ;

            services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.SwaggerDoc("v1", new
                    OpenApiInfo
                {
                    Title = "Meal Analyzer WebApi",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "Md Abu Zafar",
                        Email = "mazafar.bd@gmail.com",
                        Url = new Uri("https://www.linkedin.com/in/abu-zafar")
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("CorsPolicy");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Meal Analyzer WebApi v1"));
            }
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
