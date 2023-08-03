using System.Reflection;
using System.Text;

using InvSys.Application.Behaviors;
using InvSys.Application.Common.Interfaces;
using InvSys.Application.Common.Roles;
using InvSys.Application.Entities;
using InvSys.Application.Infrastructure;
using InvSys.Application.Infrastructure.Security;
using InvSys.Application.Infrastructure.Services;
using InvSys.Infrastructure.Common.Security;

using Mapster;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Serilog;

namespace InvSys.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuth(configuration);

        services.AddDbContext<InvSysDbContext>(options =>
                    options.UseNpgsql("Host=localhost;Database=inv_sys;User Id=postgres;password=test123")
                )
                .AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<InvSysDbContext>()
                .AddDefaultTokenProviders();

        services.AddSingleton<IDateTime, DateTimeService>();

        return services;
    }

    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SectionName, jwtSettings);

        services.AddSingleton(Options.Create(jwtSettings));

        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
          .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
          {
              ValidateIssuer = true,
              ValidateAudience = true,
              ValidateLifetime = true,
              ValidateIssuerSigningKey = true,
              IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
              ValidIssuer = jwtSettings.Issuer,
              ValidAudience = jwtSettings.Audience,
          });

        return services;
    }

    public static IHostBuilder AddSerilog(this IHostBuilder hostBuilder)
    {
        using var log = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        hostBuilder.UseSerilog(log);

        return hostBuilder;
    }


}