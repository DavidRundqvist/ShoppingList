using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;
using ShoppingList.DataAccess;
using ShoppingList.Services;

var builder = WebApplication.CreateBuilder(args);
var cfg = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<JsonSerializer>();
builder.Services.AddTransient<StoreFactory>();
builder.Services.AddTransient<SettingsPersistence>();
builder.Services.AddSingleton<DirectoryInfo>(sp => {

                                            var dataPath = cfg.GetSection("DataPath").Value;
                                            if (!Path.IsPathRooted(dataPath)) {
                                                dataPath = Path.Combine( sp.GetService<IWebHostEnvironment>().ContentRootPath, dataPath);
                                            }
                                            var dataDir = new DirectoryInfo(dataPath);
                                            Console.WriteLine($"Using data at {dataDir.FullName}");
                                            return dataDir;
                                        });
builder.Services.AddSingleton<IRepository, JsonFileRepository>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
        options => {
            options.LoginPath = new PathString("/auth/login");
            options.AccessDeniedPath = new PathString("/auth/denied");
        });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ShoppingList}/{action=Start}/{id?}")
    .WithStaticAssets();


app.Run();
