using MagicVilla_Web;
using MagicVilla_Web.Services;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Adding injection for automapper
builder.Services.AddAutoMapper(typeof(MappingConfig));

// Adding injection to register a httpClient on VillaService
builder.Services.AddHttpClient<IVillaService, VillaService>();

// Adding injection to register VillaService to DI
builder.Services.AddScoped<IVillaService, VillaService>();

// Adding injection to register a httpClient on VillaNumberService
builder.Services.AddHttpClient<IVillaNumberService, VillaNumberService>();

// Adding injection to register VillaNumberService to DI
builder.Services.AddScoped<IVillaNumberService, VillaNumberService>();

// Adding injection to register a Villa Web Security
builder.Services.AddHttpClient<IAuthService, AuthService>();

// Adding injection to register a Villa Web Security
builder.Services.AddScoped<IAuthService, AuthService>();

// Adding session to our API for consuming security in API
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(10);
    option.Cookie.HttpOnly = true;
    option.Cookie.HttpOnly = true;
});

// Adding a builder for authentication for web api
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Register";
        options.SlidingExpiration = true;
    });

// Adding services for IHttpContextAccessor in _Layout for login, logout, register
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Adding builder for default versioning to our api
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
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
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
