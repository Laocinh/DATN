using Microsoft.EntityFrameworkCore;
using DATN.Models;
namespace DATN
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            //Cau hinh ket noi
            var connectionString = builder.Configuration.GetConnectionString("AppConnection");
            builder.Services.AddDbContext<QldiemSvContext>(x => x.UseSqlServer(connectionString));

            //C?u hình s? d?ng session
            builder.Services.AddDistributedMemoryCache();

            //??ng kí d?ch v? cho HttpContextAccessor
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.Name = ".Hoang.Session";
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
            //S? d?ng session ?ã khai báo ? trên
            app.UseSession();

            app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=DashBoard}/{action=Index}/{id?}");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
