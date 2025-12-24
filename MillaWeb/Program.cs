using MillaWeb.Data;
using System.Security.Cryptography;
using System.Text;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Cookies";
    options.DefaultSignInScheme = "Cookies";
    options.DefaultChallengeScheme = "Cookies";
})
.AddCookie("Cookies", options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/Denied";
});


builder.Services.AddAuthorization();

builder.Services.AddSingleton<MillaWeb.Data.CatalogRepository>();  // kendi namespace'ine göre
builder.Services.AddSingleton<MillaWeb.Data.AuthRepository>();
builder.Services.AddSingleton<MillaWeb.Data.AdminProductsRepository>();
builder.Services.AddSingleton<MillaWeb.Data.AdminVariantsRepository>();
builder.Services.AddSingleton<MillaWeb.Data.AdminReportsRepository>();
builder.Services.AddSingleton<MillaWeb.Data.ProductRepository>();
builder.Services.AddSingleton<MillaWeb.Data.OrdersRepository>();
builder.Services.AddSingleton<MillaWeb.Data.UserAddressRepository>();
builder.Services.AddSingleton<MillaWeb.Data.ReturnsRepository>();
builder.Services.AddSingleton<MillaWeb.Data.AdminReturnsRepository>();
builder.Services.AddSingleton<MillaWeb.Data.ProductSocialRepository>();
builder.Services.AddSingleton<MillaWeb.Data.AdminOrdersRepository>();
builder.Services.AddScoped<AdminBrandsRepository>();
builder.Services.AddScoped<AdminSuppliersRepository>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


var app = builder.Build();


// pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();


app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Catalog}/{action=Index}/{id?}");

app.Run();
