using System;
using System.IO;
using Microsoft.AspNetCore.Blazor.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ShoppingList.Server.DataAccess;
using ShoppingList.Server.Services;

namespace ShoppingList.Server
{
    public class Startup
    {

        public Startup(IHostingEnvironment env)
        {
            var version = typeof(Startup).GetTypeInfo().Assembly.GetName().Version;
            Console.WriteLine($"Starting up Shopping list version {version}");

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddResponseCompression(options =>
            {
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
                {
                    MediaTypeNames.Application.Octet,
                    WasmMediaTypeNames.Application.Wasm,
                });
            });

            services.AddSingleton<DirectoryInfo>(sp => {
                var dataPath = Configuration.GetSection("DataPath").Value;
                if (!Path.IsPathRooted(dataPath))
                {
                    dataPath = Path.Combine(sp.GetService<IHostingEnvironment>().ContentRootPath, dataPath);
                }
                var dataDir = new DirectoryInfo(dataPath);
                Console.WriteLine($"Using data at {dataDir.FullName}");
                return dataDir;
            });
            services.AddSingleton<IRepository, JsonFileRepository>();
            services.AddTransient<JsonSerializer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default", template: "{controller}/{action}/{id?}");
            });

            app.UseBlazor<Client.Startup>();
        }
    }
}
