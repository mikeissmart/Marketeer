using Marketeer.Common.Configs;
using Marketeer.Core.Domain.Entities.Auth;
using Marketeer.Persistance.Database.DbContexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Marketeer.UI.Api.Security
{
    public static class ApiSecurityService
    {
        public static IServiceCollection AddApiSecurityServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            var jwtConfig = configuration.GetSection(nameof(JwtConfig)).Get<JwtConfig>();

            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;

                options.Lockout.AllowedForNewUsers = false;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;

                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;

                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                options.User.RequireUniqueEmail = true;
            })
           .AddDefaultTokenProviders()
           .AddEntityFrameworkStores<AppDbContext>()
           .AddUserManager<AppUserManager>()
           .AddSignInManager<AppSignInManager>()
           .AddRoleManager<RoleManager<AppRole>>()
           .AddRoleValidator<RoleValidator<AppRole>>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt =>
            {
                var key = Encoding.UTF8.GetBytes(jwtConfig.Secret);
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true, // this will validate the 3rd part of the jwt token using the secret that we added in the appsettings and verify we have generated the jwt token
                    IssuerSigningKey = new SymmetricSecurityKey(key), // Add the secret key to our Jwt encryption
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            return services;
        }

        /*public static void SeedSecurityServiceScope(this IServiceScope serviceScope)
        {
            var userManager = serviceScope.ServiceProvider.GetRequiredService<AppUserManager>();

            var adminUserTask = userManager.FindByNameAsync("Admin");
            adminUserTask.Wait();
            var adminUser = adminUserTask.Result;

            if (adminUser == null)
            {
                adminUser = new AppUser()
                {
                    UserName = "Admin",
                    NormalizedUserName = "ADMIN",
                    Email = "admin@admin.local",
                    NormalizedEmail = "ADMIN@ADMIN.LOCAL",
                    LockoutEnabled = false
                };
                userManager.CreateAsync(adminUser, "Admin!123").Wait();
            }

            CreateRole(userManager, 1, "Admin", adminUser);
            CreateRole(userManager, 2, "User", null);
        }

        private static void CreateRole(AppUserManager userManager, int id, string roleName, AppUser? user = null)
        {
            IList<string> userRoles = new List<string>();
            if (user != null)
            {
                var userRolesTask = userManager.GetRolesAsync(user);
                userRoles = userRolesTask.Result;
            }

            var roleTask = userManager.RoleManager.FindByIdAsync(id.ToString());
            roleTask.Wait();
            var role = roleTask.Result;

            if (role == null)
            {
                role = new AppRole()
                {
                    Name = roleName,
                    ConcurrencyStamp = (id + 1).ToString()
                };
                userManager.RoleManager.CreateAsync(role).Wait();
            }

            if (user != null && !userRoles.Any(x => x == roleName))
                userManager.AddToRoleAsync(user, roleName).Wait();
        }*/
    }
}
