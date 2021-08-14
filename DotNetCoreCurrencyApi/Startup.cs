using DotNetCoreCurrencyApi.Core;
using DotNetCoreCurrencyApi.Data;
using DotNetCoreCurrencyApi.Infrastructure;
using DotNetCoreCurrencyApi.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;

namespace DotNetCoreCurrencyApi
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DotNetCoreCurrencyApi", Version = "v1" });
            });

            services.AddDbContext<AppDatabaseContext>(options => options.UseInMemoryDatabase("Transactions"));

            services.AddAutoMapper(typeof(Startup));

            services.AddMvc().AddFluentValidation(mvcConf => mvcConf.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddTransient<IEntityGenericRepository, EntityGenericRepository<AppDatabaseContext>>();

            services.AddTransient<IBusinessLogicService, RepositoryService<IEntityGenericRepository>>();

            services.AddHttpClient(Constants.HttpClientFactoryName, client =>
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            }).SetHandlerLifetime(TimeSpan.FromMinutes(5))
              .AddPolicyHandler(GetRetryPolicy());
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode != System.Net.HttpStatusCode.OK)
                .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DotNetCoreCurrencyApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // add healthcheck
        }
    }
}
