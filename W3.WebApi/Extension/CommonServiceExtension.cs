using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using W3.Domain.Interfaces;
using W3.WebApi.Logs;
using W3.WebApi.Services.JWTToken;
using W3.WebApi.Services.Mail;

namespace W3.WebApi.Extension
{
    public static class CommonServiceExtension
    {
        public static IServiceCollection AddCommonService(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddTransient<MailService, SendGridMailService>();
            services.AddSingleton<ILoggerManager, LoggerManager>();
            return services;
        }
    }
}
