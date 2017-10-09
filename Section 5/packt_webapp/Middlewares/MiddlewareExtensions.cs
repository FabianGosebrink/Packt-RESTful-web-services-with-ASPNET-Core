using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PacktWebapp.Services;

namespace PacktWebapp.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomMiddleware>();
        }

        public static async void AddSeedData(this IApplicationBuilder app)
        {
            try
            {
                var seedDataService = app.ApplicationServices.GetRequiredService<ISeedDataService>();
                await seedDataService.EnsureSeedData();
            }
            catch (System.Exception exception)
            {

                throw;
            }
        }
    }
}
