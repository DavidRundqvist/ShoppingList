using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Newtonsoft.Json;
using ShoppingList.DataAccess;
using ShoppingList.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace ShoppingList
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
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            services.AddTransient<JsonSerializer>();
            services.AddTransient<StoreFactory>();
            services.AddTransient<SettingsPersistence>();
            services.AddSingleton<DirectoryInfo>(sp => {
                                                     var dataPath = Configuration.GetSection("DataPath").Value;
                                                     if (!Path.IsPathRooted(dataPath)) {
                                                         dataPath = Path.Combine( sp.GetService<IHostingEnvironment>().ContentRootPath, dataPath);
                                                     }
                                                     var dataDir = new DirectoryInfo(dataPath);
                                                     Console.WriteLine($"Using data at {dataDir.FullName}");
                                                     return dataDir;
                                                 });
            services.AddSingleton<IRepository, JsonFileRepository>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                    options => {
                        options.LoginPath = new PathString("/auth/login");
                        options.AccessDeniedPath = new PathString("/auth/denied");
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();


            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=ShoppingList}/{action=Start}/{id?}");
            });
            

            Console.WriteLine("Configuration finished");
        }
    }
}
