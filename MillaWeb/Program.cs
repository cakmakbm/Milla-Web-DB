var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication("Cookies")
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


app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Catalog}/{action=Index}/{id?}");

app.Run();
