using MagicVilla_Web;
using MagicVilla_Web.Services;
using MagicVilla_Web.Services.IServices;

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
