using WebApplication6.Repositories;
using Microsoft.EntityFrameworkCore;
using WebApplication6.Models;
using WebApplication6.Repositories;

var builder = WebApplication.CreateBuilder(args);

// =============================
// ??ng ký các d?ch v? vào container
// =============================

// 1. ??ng ký DbContext và c?u hình k?t n?i SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. ??ng ký Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Th?i gian session h?t h?n
    options.Cookie.HttpOnly = true;                // B?o m?t cookie
    options.Cookie.IsEssential = true;             // Cookie là c?n thi?t
});

// 3. ??ng ký Dependency Injection cho Repository
builder.Services.AddScoped<IProductRepository, EFProductRepository>();
builder.Services.AddScoped<ICategoryRepository, EFCategoryRepository>();

// 4. ??ng ký Controller và Views
builder.Services.AddControllersWithViews();

var app = builder.Build();

// =============================
// C?u hình Middleware pipeline
// =============================

// 1. X? lý l?i cho môi tr??ng Production
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // HTTP Strict Transport Security
}

// 2. Middleware c? b?n
app.UseHttpsRedirection(); // Chuy?n h??ng HTTP sang HTTPS
app.UseStaticFiles();      // Ph?c v? file t?nh nh? CSS, JS, images

// 3. Kích ho?t Session và ??nh tuy?n
app.UseRouting();    // Middleware ??nh tuy?n
app.UseSession();    // Kích ho?t Session
app.UseAuthorization(); // Middleware xác th?c quy?n

// 4. C?u hình ??nh tuy?n cho các Controller
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}"); // ??t Login là trang m?c ??nh

// 5. Ch?y ?ng d?ng
app.Run();
