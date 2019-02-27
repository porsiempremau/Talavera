using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using TalaveraWeb.Models;
using TalaveraWeb.Models.MiBD;

namespace TalaveraWeb.Models
{
    // Puede agregar datos del perfil del usuario agregando más propiedades a la clase ApplicationUser. Para más información, visite http://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Nombre completo"), Required, StringLength(200)]
        public string NombreCompleto { get; set; }

        [Display(Name = "Nombre corto"), Required, StringLength(20, ErrorMessage = "Considera usar un nombre corto de meno de 20 caracteres.")]
        public string Nick { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Tenga en cuenta que el valor de authenticationType debe coincidir con el definido en CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Agregar aquí notificaciones personalizadas de usuario
            return userIdentity;
        }
    }

    public  class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string roleName) : base(roleName) { }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<EntregaPellas> EntregaPellas { get; set; }
        public DbSet<BarroMaestra> BarroMaestra { get; set; }
        public DbSet<BarroMovimientos> BarroMovimientos { get; set; }        
        public DbSet<PreparacionBarro> PreparacionBarro { get; set; }
        public DbSet<PreparacionPellas> PreparacionPellas { get; set; }
        public DbSet<prepBarro_prepPellas> prepBarro_prepPellas { get; set; }
        public DbSet<Provedores> Provedores { get; set; }
        public DbSet<Recuperados> Recuperados { get; set; }
        public DbSet<Sucursales> Sucursales { get; set; }

        public System.Data.Entity.DbSet<TalaveraWeb.Models.PreparacionPellasConPreBar> PreparacionPellasConPreBars { get; set; }

        public DbSet<CatalogoTalavera> CatalogoTalavera { get; set; }

        public DbSet<PersonalTalavera> PersonalTalavera { get; set; }
    }
}