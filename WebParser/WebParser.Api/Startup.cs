using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebParser.Api.BackgroundProcessor;
using WebParser.Api.Common;
using WebParser.Api.Scan;
using WebParser.Api.Storage;
using Newtonsoft.Json.Converters;

namespace WebParser.Api
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
            services.AddControllers()
                 //Suppressing invalid model state response. This is to avoid different response structure 
                 .ConfigureApiBehaviorOptions(options =>
                 {
                     options.SuppressModelStateInvalidFilter = true;
                 })
            .AddNewtonsoftJson(options => options.SerializerSettings.Converters.Add(new StringEnumConverter()));

            services.AddAutoMapper(typeof(Startup));
            services.AddSingleton<IScanProcessor, ScanProcessor>();
            services.AddHostedService<BackgroundQueueService>();
            services.AddSingleton<IBackgroundQueue, BackgroundQueue>();
            services.AddSingleton<IMemoryCache, MemoryCache>();
            services.AddSingleton<IStorageManager, StorageManager>();
            services.AddSingleton<IDataExtractor, DataExtractor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Custom exception middleware for handling all errors in common place 
            app.ConfigureCustomExceptionMiddleware();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
