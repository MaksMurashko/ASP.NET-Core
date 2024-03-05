using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniqueProducts.Data;
using UniqueProducts.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Globalization;

namespace UniqueProducts
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var services = builder.Services;

            // ��������� ����������� ��� ������� � �� � �������������� EF
            string connection = builder.Configuration.GetConnectionString("SqlServerConnection");
            services.AddDbContext<UniqueProductsContext>(options => options.UseSqlServer(connection));

            services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 5;
            })
            .AddRoles<IdentityRole>()
            .AddDefaultUI()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<UniqueProductsContext>();

            services.AddDistributedMemoryCache();
            services.AddSession();
         
            //������������� MVC
            services.AddControllersWithViews();
            services.AddRazorPages();
            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // ��������� ��������� ������
            app.UseSession();
            // ��������� ��������� middleware ��� ������������� ���� ������ � ���������� ������������� ����
            app.UseDbInitializer();

            CultureInfo culture = new("ru-RU");
            culture.NumberFormat.NumberDecimalSeparator = ".";
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            app.UseRouting();

            // ������������� Identity
            app.UseAuthentication();
            app.UseAuthorization();
            // ������������� ����������� ���������
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
            app.Run();
        }
    }
}