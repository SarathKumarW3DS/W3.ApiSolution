using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using W3.Infrastructure.DataContext;

namespace W3.WebApi.Extension
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<Context>(options =>
            options.UseSqlServer(config.GetConnectionString("DbConnection")));
            return services;
        }
    }
}
