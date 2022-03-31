using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;

namespace Mre.Sb.Base.EntityFrameworkCore
{
    public static class GestionPermisosDbContextModelBuilderExtensions
    {
        public static void ConfigurarGestionPermisos(
            [NotNull] this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<PermissionGrant>(b =>
            {
                b.ToTable(AbpPermissionManagementDbProperties.DbTablePrefix + "PermisoOtorgado", AbpPermissionManagementDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.Name).HasColumnName("Nombre").HasMaxLength(PermissionGrantConsts.MaxNameLength).IsRequired();
                b.Property(x => x.ProviderName).HasColumnName("Proveedor").HasMaxLength(PermissionGrantConsts.MaxProviderNameLength).IsRequired();
                b.Property(x => x.ProviderKey).HasColumnName("ProveedorClave").HasMaxLength(PermissionGrantConsts.MaxProviderKeyLength).IsRequired();

                b.HasIndex(x => new { x.TenantId, x.Name, x.ProviderName, x.ProviderKey }).IsUnique(true);

                b.ApplyObjectExtensionMappings();
            });

            builder.TryConfigureObjectExtensions<PermissionManagementDbContext>();
        }
    }
}
