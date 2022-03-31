using Microsoft.EntityFrameworkCore;
using Mre.Sb.Base.Identidad;
using System;
using System.Diagnostics.CodeAnalysis;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Users.EntityFrameworkCore;

namespace Mre.Sb.Base.EntityFrameworkCore
{
    public static class IdentidadDbContextModelBuilderExtensions
    {
        public static void ConfigurarIdentidad([NotNull] this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<IdentityUser>(b =>
            {
                b.ToTable(AbpIdentityDbProperties.DbTablePrefix + "Usuario", AbpIdentityDbProperties.DbSchema);

                b.ConfigureByConvention();
                b.ConfigureAbpUser();

                b.Property(x => x.UserName).HasColumnName("Usuario");
                b.Property(x => x.Name).HasColumnName("Nombre");
                b.Property(x => x.Surname).HasColumnName("Apellido");  
                b.Property(x => x.EmailConfirmed).HasColumnName("EmailConfirmado");
                b.Property(x => x.PhoneNumber).HasColumnName("Telefono");
                b.Property(x => x.PhoneNumberConfirmed).HasColumnName("TelefonoConfirmado");
                b.Property(x => x.LockoutEnd).HasColumnName("BloqueoFin"); 

                  

                b.Property(u => u.NormalizedUserName).IsRequired()
                    .HasMaxLength(IdentityUserConsts.MaxNormalizedUserNameLength)
                    .HasColumnName("UsuarioNormalizado");
                b.Property(u => u.NormalizedEmail).IsRequired()
                    .HasMaxLength(IdentityUserConsts.MaxNormalizedEmailLength)
                    .HasColumnName("EmailNormalizado");
                b.Property(u => u.PasswordHash).HasMaxLength(IdentityUserConsts.MaxPasswordHashLength)
                    .HasColumnName("ClaveHash");
                b.Property(u => u.SecurityStamp).IsRequired().HasMaxLength(IdentityUserConsts.MaxSecurityStampLength)
                    .HasColumnName(nameof(IdentityUser.SecurityStamp));
                b.Property(u => u.TwoFactorEnabled).HasDefaultValue(false)
                    .HasColumnName("DobleFactorActivo");
                b.Property(u => u.LockoutEnabled).HasDefaultValue(false)
                    .HasColumnName("BloqueoActivo");

                b.Property(u => u.IsExternal).IsRequired().HasDefaultValue(false)
                    .HasColumnName("AutExterna");

                b.Property(u => u.AccessFailedCount)
                    .If(!builder.IsUsingOracle(), p => p.HasDefaultValue(0))
                    .HasColumnName("TotalAccesoFallidos");

                //Personalizaciones. Estructura Usuarios
                b.Property(u => u.IsActive).HasColumnName("EsActivo").HasDefaultValue(true);

                b.Property(u => u.UserType)
                    .HasColumnName("Tipo");
                  
                b.Property(u => u.Code)
                   .HasMaxLength(IdentityUserConsts.MaxCodeLength)
                   .HasColumnName("Codigo");
                 
                b.Property(u => u.ShouldChangePasswordOnNextLogin).HasDefaultValue(false)
                   .HasColumnName("CambiarClaveSigAcceso");

                b.HasMany(u => u.Claims).WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
                b.HasMany(u => u.Logins).WithOne().HasForeignKey(ul => ul.UserId).IsRequired();
                b.HasMany(u => u.Roles).WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
                b.HasMany(u => u.Tokens).WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
                b.HasMany(u => u.OrganizationUnits).WithOne().HasForeignKey(ur => ur.UserId).IsRequired();

                b.HasIndex(u => u.NormalizedUserName);
                b.HasIndex(u => u.NormalizedEmail);
                b.HasIndex(u => u.UserName);
                b.HasIndex(u => u.Email);

                b.ApplyObjectExtensionMappings();
            });

            builder.Entity<IdentityUserClaim>(b =>
            {
                b.ToTable(AbpIdentityDbProperties.DbTablePrefix + "UsuarioClaim", AbpIdentityDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.Id).ValueGeneratedNever();

                b.Property(uc => uc.UserId).HasColumnName("UsuarioId");

                b.Property(uc => uc.ClaimType).HasColumnName("ClaimTipo").HasMaxLength(IdentityUserClaimConsts.MaxClaimTypeLength).IsRequired();
                b.Property(uc => uc.ClaimValue).HasColumnName("ClaimValor").HasMaxLength(IdentityUserClaimConsts.MaxClaimValueLength);

                b.HasIndex(uc => uc.UserId);

                b.ApplyObjectExtensionMappings();
            });

            builder.Entity<IdentityUserRole>(b =>
            {
                b.ToTable(AbpIdentityDbProperties.DbTablePrefix + "UsuarioRol", AbpIdentityDbProperties.DbSchema);

                b.ConfigureByConvention();
                
                b.Property(uc => uc.UserId).HasColumnName("UsuarioId");
                b.Property(uc => uc.RoleId).HasColumnName("RolId");

                b.HasKey(ur => new { ur.UserId, ur.RoleId });

                b.HasOne<IdentityRole>().WithMany().HasForeignKey(ur => ur.RoleId).IsRequired();
                b.HasOne<IdentityUser>().WithMany(u => u.Roles).HasForeignKey(ur => ur.UserId).IsRequired();

                b.HasIndex(ur => new { ur.RoleId, ur.UserId });

                b.ApplyObjectExtensionMappings();
            });

            builder.Entity<IdentityUserLogin>(b =>
            {
                b.ToTable(AbpIdentityDbProperties.DbTablePrefix + "UsuarioLogin", AbpIdentityDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(uc => uc.UserId).HasColumnName("UsuarioId");

                b.HasKey(x => new { x.UserId, x.LoginProvider });

                b.Property(ul => ul.LoginProvider).HasColumnName("LoginProveedor").HasMaxLength(IdentityUserLoginConsts.MaxLoginProviderLength)
                    .IsRequired();
                b.Property(ul => ul.ProviderKey).HasColumnName("ProveedorClave").HasMaxLength(IdentityUserLoginConsts.MaxProviderKeyLength)
                    .IsRequired();
                b.Property(ul => ul.ProviderDisplayName).HasColumnName("ProveedorNombre")
                    .HasMaxLength(IdentityUserLoginConsts.MaxProviderDisplayNameLength);

                b.HasIndex(l => new { l.LoginProvider, l.ProviderKey });

                b.ApplyObjectExtensionMappings();
            });

            builder.Entity<IdentityUserToken>(b =>
            {
                b.ToTable(AbpIdentityDbProperties.DbTablePrefix + "UsuarioToken", AbpIdentityDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(uc => uc.UserId).HasColumnName("UsuarioId");
                b.Property(uc => uc.LoginProvider).HasColumnName("LoginProveedor");
                b.Property(uc => uc.Name).HasColumnName("Nombre");


                b.HasKey(l => new { l.UserId, l.LoginProvider, l.Name });

                b.Property(ul => ul.LoginProvider).HasMaxLength(IdentityUserTokenConsts.MaxLoginProviderLength)
                    .IsRequired();
                b.Property(ul => ul.Name).HasMaxLength(IdentityUserTokenConsts.MaxNameLength).IsRequired();

                b.ApplyObjectExtensionMappings();
            });

            builder.Entity<IdentityRole>(b =>
            {
                b.ToTable(AbpIdentityDbProperties.DbTablePrefix + "Rol", AbpIdentityDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(r => r.Name).HasColumnName("Nombre").IsRequired().HasMaxLength(IdentityRoleConsts.MaxNameLength);
                b.Property(r => r.NormalizedName).HasColumnName("NombreNormalizado").IsRequired().HasMaxLength(IdentityRoleConsts.MaxNormalizedNameLength);
                b.Property(r => r.IsDefault).HasColumnName("EsDefecto");
                b.Property(r => r.IsStatic).HasColumnName("EsEstitico");
                b.Property(r => r.IsPublic).HasColumnName("EsPublico");

                b.HasMany(r => r.Claims).WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();

                b.HasIndex(r => r.NormalizedName);

                b.ApplyObjectExtensionMappings();
            });

            builder.Entity<IdentityRoleClaim>(b =>
            {
                b.ToTable(AbpIdentityDbProperties.DbTablePrefix + "RolClaim", AbpIdentityDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(uc => uc.RoleId).HasColumnName("RolId");

                b.Property(x => x.Id).ValueGeneratedNever();

                b.Property(uc => uc.ClaimType).HasColumnName("ClaimTipo").HasMaxLength(IdentityRoleClaimConsts.MaxClaimTypeLength).IsRequired();
                b.Property(uc => uc.ClaimValue).HasColumnName("ClaimValor").HasMaxLength(IdentityRoleClaimConsts.MaxClaimValueLength);

                b.HasIndex(uc => uc.RoleId);

                b.ApplyObjectExtensionMappings();
            });

            if (builder.IsHostDatabase())
            {
                builder.Entity<IdentityClaimType>(b =>
                {
                    b.ToTable(AbpIdentityDbProperties.DbTablePrefix + "ClaimTipo", AbpIdentityDbProperties.DbSchema);

                    b.ConfigureByConvention();

                    b.Property(uc => uc.Required).HasColumnName("Requerido");
                    b.Property(uc => uc.IsStatic).HasColumnName("EsEstatico");
                    b.Property(uc => uc.Description).HasColumnName("Descripcion");
                    b.Property(uc => uc.ValueType).HasColumnName("ValorTipo");




                    b.Property(uc => uc.Name).HasColumnName("Nombre").HasMaxLength(IdentityClaimTypeConsts.MaxNameLength)
                        .IsRequired(); // make unique
                    b.Property(uc => uc.Regex).HasColumnName("ExpRegular").HasMaxLength(IdentityClaimTypeConsts.MaxRegexLength);
                    b.Property(uc => uc.RegexDescription).HasColumnName("ExpRegularDesc").HasMaxLength(IdentityClaimTypeConsts.MaxRegexDescriptionLength);
                    b.Property(uc => uc.Description).HasColumnName("Descripcion").HasMaxLength(IdentityClaimTypeConsts.MaxDescriptionLength);

                    b.ApplyObjectExtensionMappings();
                });
            }

            builder.Entity<OrganizationUnit>(b =>
            {
                b.ToTable(AbpIdentityDbProperties.DbTablePrefix + "UniOrganizacional", AbpIdentityDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(ou => ou.Code).IsRequired().HasMaxLength(OrganizationUnitConsts.MaxCodeLength)
                    .HasColumnName("Codigo");
                b.Property(ou => ou.DisplayName).IsRequired().HasMaxLength(OrganizationUnitConsts.MaxDisplayNameLength)
                    .HasColumnName("NombreVisualizar");

                b.HasMany<OrganizationUnit>().WithOne().HasForeignKey(ou => ou.ParentId);
                b.HasMany(ou => ou.Roles).WithOne().HasForeignKey(our => our.OrganizationUnitId).IsRequired();

                b.HasIndex(ou => ou.Code);

                b.ApplyObjectExtensionMappings();
            });

            builder.Entity<OrganizationUnitRole>(b =>
            {
                b.ToTable(AbpIdentityDbProperties.DbTablePrefix + "UniOrganizacionalRol", AbpIdentityDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(uc => uc.RoleId).HasColumnName("RolId");
                b.Property(uc => uc.OrganizationUnitId).HasColumnName("UniOrgId");


                b.HasKey(ou => new { ou.OrganizationUnitId, ou.RoleId });

                b.HasOne<IdentityRole>().WithMany().HasForeignKey(ou => ou.RoleId).IsRequired();

                b.HasIndex(ou => new { ou.RoleId, ou.OrganizationUnitId });

                b.ApplyObjectExtensionMappings();
            });

            builder.Entity<IdentityUserOrganizationUnit>(b =>
            {
                b.ToTable(AbpIdentityDbProperties.DbTablePrefix + "UniOrganizacionalUsuario", AbpIdentityDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(uc => uc.UserId).HasColumnName("UsuarioId");
                b.Property(uc => uc.OrganizationUnitId).HasColumnName("UniOrgId");


                b.HasKey(ou => new { ou.OrganizationUnitId, ou.UserId });

                b.HasOne<OrganizationUnit>().WithMany().HasForeignKey(ou => ou.OrganizationUnitId).IsRequired();

                b.HasIndex(ou => new { ou.UserId, ou.OrganizationUnitId });

                b.ApplyObjectExtensionMappings();
            });

            builder.Entity<IdentitySecurityLog>(b =>
            {
                b.ToTable(AbpIdentityDbProperties.DbTablePrefix + "LogSeguridad", AbpIdentityDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.TenantName).HasMaxLength(IdentitySecurityLogConsts.MaxTenantNameLength);

                b.Property(x => x.ApplicationName).HasColumnName("Aplicacion").HasMaxLength(IdentitySecurityLogConsts.MaxApplicationNameLength);
                b.Property(x => x.Identity).HasColumnName("Identidad").HasMaxLength(IdentitySecurityLogConsts.MaxIdentityLength);
                b.Property(x => x.Action).HasColumnName("Accion").HasMaxLength(IdentitySecurityLogConsts.MaxActionLength);

                b.Property(x => x.UserName).HasColumnName("Usuario").HasMaxLength(IdentitySecurityLogConsts.MaxUserNameLength);

                b.Property(x => x.ClientIpAddress).HasColumnName("ClienteIp").HasMaxLength(IdentitySecurityLogConsts.MaxClientIpAddressLength);
                b.Property(x => x.ClientId).HasColumnName("ClienteId").HasMaxLength(IdentitySecurityLogConsts.MaxClientIdLength);
                b.Property(x => x.CorrelationId).HasColumnName("CorrelacionId").HasMaxLength(IdentitySecurityLogConsts.MaxCorrelationIdLength);
                b.Property(x => x.BrowserInfo).HasColumnName("NavegadorInfo").HasMaxLength(IdentitySecurityLogConsts.MaxBrowserInfoLength);

                b.HasIndex(x => new { x.TenantId, x.ApplicationName });
                b.HasIndex(x => new { x.TenantId, x.Identity });
                b.HasIndex(x => new { x.TenantId, x.Action });
                b.HasIndex(x => new { x.TenantId, x.UserId });

                b.ApplyObjectExtensionMappings();
            });

            if (builder.IsHostDatabase())
            {
                builder.Entity<IdentityLinkUser>(b =>
                {
                    b.ToTable(AbpIdentityDbProperties.DbTablePrefix + "UsuarioEnlace", AbpIdentityDbProperties.DbSchema);

                    b.ConfigureByConvention();

                    b.Property(uc => uc.SourceUserId).HasColumnName("UsuarioOrigenId");
                    b.Property(uc => uc.TargetUserId).HasColumnName("UsuarioDestinoId");


                    b.HasIndex(x => new {
                        UserId = x.SourceUserId,
                        TenantId = x.SourceTenantId,
                        LinkedUserId = x.TargetUserId,
                        LinkedTenantId = x.TargetTenantId
                    }).IsUnique();

                    b.ApplyObjectExtensionMappings();
                });
            }


            //Entidades Nuevas
            builder.Entity<UsuarioHistorico>(b =>
            {
                b.ToTable(AbpIdentityDbProperties.DbTablePrefix + "UsuarioHistorico", AbpIdentityDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(u => u.ClaveHash).HasMaxLength(IdentityUserConsts.MaxPasswordHashLength);

                b.HasOne<IdentityUser>().WithMany().HasForeignKey(ou => ou.UsuarioId).IsRequired();
  
                b.ApplyObjectExtensionMappings();
            });

            builder.TryConfigureObjectExtensions<IdentityDbContext>();
        }
    }
}
