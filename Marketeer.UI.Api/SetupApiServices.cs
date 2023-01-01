using Marketeer.Core.Domain.Entities.Auth;
using Marketeer.UI.Api.Security;

namespace Marketeer.UI.Api
{
    public static class SetupApiServices
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddApiSecurityServices(configuration);

            return services;
        }

        public static void SeedSecurityServiceScope(this IServiceScope serviceScope)
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
        }
    }
}
