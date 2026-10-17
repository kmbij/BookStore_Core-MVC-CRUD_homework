using BookStore.Data;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// 第一次執行時自動建立資料庫並塞入範例資料
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    if (!db.Books.Any())
    {
        db.Books.AddRange(
            new Book { Title = "C# 入門指南", Author = "王小明", Category = "程式設計", Price = 580, Stock = 20, PublishDate = new DateTime(2023, 3, 1) },
            new Book { Title = "ASP.NET Core 實戰", Author = "陳大華", Category = "程式設計", Price = 720, Stock = 15, PublishDate = new DateTime(2024, 6, 15) },
            new Book { Title = "資料庫設計原理", Author = "林美麗", Category = "資料庫", Price = 650, Stock = 8, PublishDate = new DateTime(2022, 11, 20) });
        db.SaveChanges();
    }
}

app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(name: "default", pattern: "{controller=Books}/{action=Index}/{id?}");
app.Run();
