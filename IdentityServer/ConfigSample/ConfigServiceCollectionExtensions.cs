using IdentityServer.Services.JWT;
using IdentityServer_DAL;
using IdentityServer_DAL.Entities;
using IdentityServer_DAL.Repos.Contracts;
using IdentityServer_DAL.Repos.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace IdentityServer.ConfigSample
{
    public static class ConfigServiceCollectionExtensions
    {
        public static IServiceCollection AddDALDependencyGroup(
             this IServiceCollection services,
             ConfigurationManager config)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            var conStr = config.GetConnectionString("SQL")
                ?? throw new NullReferenceException("No SQL Connection String in configs.");

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(conStr));

            return services;
        }

        public static IServiceCollection AddMicrosoftIdentityServices(
             this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(options => {
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = false; // Don't allow lockout for new users
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddSignInManager();

            return services;
        }

        /// <summary> Sets JWT Authentication. </summary>
        public static IServiceCollection AddTokenConfigs(
            this IServiceCollection services,
            IConfiguration config)
        {
            services.AddScoped<ITokenService, TokenService>();

            var jwtSettings = config.GetSection("JwtSettings");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,    //For token expiration time
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!))
                };
            });

            return services;
        }
    }
}
