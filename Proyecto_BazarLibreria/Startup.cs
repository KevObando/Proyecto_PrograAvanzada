using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Owin;
using Proyecto_BazarLibreria.Models;

[assembly: OwinStartupAttribute(typeof(Proyecto_BazarLibreria.Startup))]
namespace Proyecto_BazarLibreria
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            InitializeRolesAndAdminUser();
        }

        private void InitializeRolesAndAdminUser()
        {
            using (var context = new ApplicationDbContext())
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                // Crear el rol "Admin" si no existe
                if (!roleManager.RoleExists("Admin"))
                {
                    var role = new IdentityRole("Admin");
                    roleManager.Create(role);
                }

                // Crear el rol "User" si no existe
                if (!roleManager.RoleExists("User"))
                {
                    var role = new IdentityRole("User");
                    roleManager.Create(role);
                }

                // Crear el usuario administrador si no existe
                if (userManager.FindByName("admin@domain.com") == null)
                {
                    var adminUser = new ApplicationUser
                    {
                        UserName = "admin@domain.com",
                        Email = "admin@domain.com"
                    };

                    var adminPassword = "Admin123!";
                    var userCreationResult = userManager.Create(adminUser, adminPassword);

                    if (userCreationResult.Succeeded)
                    {
                        // Asignar el rol "Admin" al usuario
                        userManager.AddToRole(adminUser.Id, "Admin");
                    }
                }
            }
        }
    }
}