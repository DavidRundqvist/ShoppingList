using System;
using System.IO;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;
using ShoppingList.DataAccess;
using ShoppingList.Models;
using ShoppingList.Services;
using System.Reflection;

namespace ShoppingList
{
    public class Startup
    {
        public Startup() {
            var version = typeof(Startup).GetTypeInfo().Assembly.GetName().Version;
            Console.WriteLine($"Starting up Shopping list version {version}");
        }

        //public Startup(IHostingEnvironment env)
        //{
        //    // Set up configuration sources.
        //    var builder = new ConfigurationBuilder()
        //        .AddJsonFile("appsettings.json")
        //        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

        //    if (env.IsDevelopment())
        //    {
        //        // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
        //        builder.AddUserSecrets();
        //    }

        //    builder.AddEnvironmentVariables();
        //    Configuration = builder.Build();
        //}

        //public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //// Add framework services.
            //services.AddEntityFramework()
            //    .AddSqlServer()
            //    .AddDbContext<ApplicationDbContext>(options =>
            //        options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>()
            //    .AddDefaultTokenProviders();

            services.AddMvc();

            // Add application services.
            services.AddTransient<JsonSerializer>();
            services.AddTransient<StoreFactory>();
            services.AddTransient<TestData>();
            services.AddTransient<SettingsPersistence>();
            services.AddSingleton<DirectoryInfo>(sp => {
                                                   var dataPath = "";
                                                   if (sp.GetService<IHostingEnvironment>().IsDevelopment()) {
                                                       var root = sp.GetService<IApplicationEnvironment>().ApplicationBasePath;
                                                       dataPath = Path.Combine(root, "../../../ShoppingListData");
                                                   }
                                                   else {
                                                       dataPath = @"/home/david/asp/ShoppingListData";
                                                   }
                                                   var dataDir = new DirectoryInfo(dataPath);
                                                   Console.WriteLine($"Using data at {dataDir.FullName}");
                                                     return dataDir;
                                               });
            services.AddTransient<IRepository, JsonFileRepository>();
            //services.AddSingleton<IRepository>(sp => {
            //                                       var result = new MemoryRepository();
            //                                       var env = sp.GetService<IHostingEnvironment>();
            //                                       if (env.IsDevelopment()) {
            //                                           sp.GetService<TestData>().InsertTestData(result);
            //                                       }
            //                                       return result;
            //                                   });

            //services.AddTransient<IEmailSender, AuthMessageSender>();
            //services.AddTransient<ISmsSender, AuthMessageSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();

            //if (env.IsDevelopment())
            //{
            //    app.UseBrowserLink();
            //    app.UseDeveloperExceptionPage();
            //    app.UseDatabaseErrorPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");

            //    // For more details on creating database during deployment see http://go.microsoft.com/fwlink/?LinkID=615859
            //    try
            //    {
            //        using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
            //            .CreateScope())
            //        {
            //            serviceScope.ServiceProvider.GetService<ApplicationDbContext>()
            //                 .Database.Migrate();
            //        }
            //    }
            //    catch { }
            //}


            
            app.UseIISPlatformHandler(options => options.AuthenticationDescriptions.Clear());

            app.UseStaticFiles();

            //app.UseIdentity();

            // To configure external authentication please see http://go.microsoft.com/fwlink/?LinkID=532715
            app.UseDeveloperExceptionPage();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=ShoppingList}/{action=Start}/{id?}");

                /*
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Start}/{id?}");*/
            });

            Console.WriteLine("Configuration finished");
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
