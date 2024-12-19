using WebApplication6.Repositories;
using Microsoft.EntityFrameworkCore;
using WebApplication6.Models;
using WebApplication6.Repositories;

var builder = WebApplication.CreateBuilder(args);

// =============================
// ??ng k� c�c d?ch v? v�o container
// =============================

// 1. ??ng k� DbContext v� c?u h�nh k?t n?i SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. ??ng k� Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Th?i gian session h?t h?n
    options.Cookie.HttpOnly = true;                // B?o m?t cookie
    options.Cookie.IsEssential = true;             // Cookie l� c?n thi?t
});

// 3. ??ng k� Dependency Injection cho Repository
builder.Services.AddScoped<IProductRepository, EFProductRepository>();
builder.Services.AddScoped<ICategoryRepository, EFCategoryRepository>();

// 4. ??ng k� Controller v� Views
builder.Services.AddControllersWithViews();

var app = builder.Build();

// =============================
// C?u h�nh Middleware pipeline
// =============================

// 1. X? l� l?i cho m�i tr??ng Production
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // HTTP Strict Transport Security
}

// 2. Middleware c? b?n
app.UseHttpsRedirection(); // Chuy?n h??ng HTTP sang HTTPS
app.UseStaticFiles();      // Ph?c v? file t?nh nh? CSS, JS, images

// 3. K�ch ho?t Session v� ??nh tuy?n
app.UseRouting();    // Middleware ??nh tuy?n
app.UseSession();    // K�ch ho?t Session
app.UseAuthorization(); // Middleware x�c th?c quy?n

// 4. C?u h�nh ??nh tuy?n cho c�c Controller
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}"); // ??t Login l� trang m?c ??nh

// 5. Ch?y ?ng d?ng
app.Run();
