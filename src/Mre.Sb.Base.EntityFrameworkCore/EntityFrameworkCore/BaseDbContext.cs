using Audit.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Mre.Sb.AuditoriaConf.AuditoriaConf;
using Mre.Sb.AuditoriaConf.EntityFrameworkCore;
using Mre.Sb.Base.Identidad;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace Mre.Sb.Base.EntityFrameworkCore
{


    [ReplaceDbContext(typeof(IIdentityDbContext))]
    [ReplaceDbContext(typeof(ITenantManagementDbContext))]
    [ReplaceDbContext(typeof(ISettingManagementDbContext))]
    [ReplaceDbContext(typeof(IPermissionManagementDbContext))]
    [ReplaceDbContext(typeof(IAuditoriaConfDbContext))]
    [ConnectionStringName("Default")]
    public class BaseDbContext : 
        AbpDbContext<BaseDbContext>,
        IIdentityDbContext,
        ITenantManagementDbContext,
        ISettingManagementDbContext,
        IPermissionManagementDbContext,
        IAuditoriaConfDbContext
    {
        
        public DbSet<Setting> Settings { get; set; }

        #region Gestion Permisos
        
        public DbSet<PermissionGrant> PermissionGrants { get; set; }

        #endregion Gestion Permisos


        #region Auditar

        public DbSet<Auditable> Auditable { get; set; }

        public DbSet<Categoria> Categoria { get; set; }

        public DbSet<Auditar> Auditar { get; set; }

        #endregion Auditar


        #region Entities from the modules

        //Identity
        public DbSet<IdentityUser> Users { get; set; }
        public DbSet<IdentityRole> Roles { get; set; }
        public DbSet<IdentityClaimType> ClaimTypes { get; set; }
        public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
        public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
        public DbSet<IdentityLinkUser> LinkUsers { get; set; }
        
        // Tenant Management
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

        #endregion

        public DbSet<UsuarioHistorico> UsuarioHistorico { get; set; }


        public BaseDbContext(DbContextOptions<BaseDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /* Include modules to your migration db context */

            builder.ConfigurarGestionPermisos(); 
            builder.ConfigurarGestionConfiguracion(); 
            builder.ConfigurarIdentidad(); 
            builder.ConfigureFeatureManagement();
            builder.ConfigureTenantManagement();


            builder.ConfigureAuditoriaConf();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
            optionsBuilder.AddInterceptors(new AuditSaveChangesInterceptor());
        }
    }
}
