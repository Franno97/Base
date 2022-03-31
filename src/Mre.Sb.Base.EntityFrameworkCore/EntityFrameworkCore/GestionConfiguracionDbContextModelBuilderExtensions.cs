using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.SettingManagement;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace Mre.Sb.Base.EntityFrameworkCore
{
    public static class GestionConfiguracionDbContextModelBuilderExtensions
    {
        public static void ConfigurarGestionConfiguracion(
            [NotNull] this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            if (builder.IsTenantOnlyDatabase())
            {
                return;
            }

            builder.Entity<Setting>(b =>
            {
                b.ToTable(AbpSettingManagementDbProperties.DbTablePrefix + "Configuracion", AbpSettingManagementDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.Name).HasColumnName("Nombre").HasMaxLength(SettingConsts.MaxNameLength).IsRequired();

                if (builder.IsUsingOracle()) { SettingConsts.MaxValueLengthValue = 2000; }
                b.Property(x => x.Value).HasColumnName("Valor").HasMaxLength(SettingConsts.MaxValueLengthValue).IsRequired();

                b.Property(x => x.ProviderName).HasColumnName("Proveedor").HasMaxLength(SettingConsts.MaxProviderNameLength);
                b.Property(x => x.ProviderKey).HasColumnName("ProveedorClave").HasMaxLength(SettingConsts.MaxProviderKeyLength);

                b.HasIndex(x => new { x.Name, x.ProviderName, x.ProviderKey }).IsUnique(true);

                b.ApplyObjectExtensionMappings();
            });

            builder.TryConfigureObjectExtensions<SettingManagementDbContext>();
        }
    }
}
