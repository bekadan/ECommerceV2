using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Application.Core.Abstractions.Common;
using ProductService.Application.Core.Abstractions.Data;
using ProductService.Infrastructure.Common;
using ProductService.Infrastructure.Data;

namespace ProductService.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString(ConnectionString.SettingsKey);

        if (connectionString?.Length > 0)
        {
            services.AddSingleton(new ConnectionString(connectionString));

            services.AddDbContext<ProductDbContext>(options =>
            {
                options.UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention();
            });
        }

        services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();

        services.AddScoped<IDbExecutor, DbExecutor>();

        services.AddScoped<IDbContext>(serviceProvider => serviceProvider.GetRequiredService<ProductDbContext>());

        services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<ProductDbContext>());

        services.AddTransient<IDateTime, MachineDateTime>();
    }
}
