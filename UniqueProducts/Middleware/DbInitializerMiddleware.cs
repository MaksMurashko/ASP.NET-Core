using UniqueProducts.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace UniqueProducts.Middleware
{
    public class DbInitializerMiddleware
    {
        private readonly RequestDelegate _next;
        public DbInitializerMiddleware(RequestDelegate next)
        {
            // инициализация базы данных 
            _next = next;
        }
        public Task Invoke(HttpContext context)
        {
            if (!(context.Session.Keys.Contains("starting")))
            {
                DbInitializer.Initialize(context.RequestServices.GetRequiredService<UniqueProductsContext>(), context).Wait();
                context.Session.SetString("starting", "Yes");
            }

            // Вызов следующего делегата / компонента middleware в конвейере
            return _next.Invoke(context);
        }
    }
    public static class DbInitializerExtensions
    {
        public static IApplicationBuilder UseDbInitializer(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DbInitializerMiddleware>();
        }

    }

}