using Microsoft.EntityFrameworkCore;
using Mre.Sb.AuditoriaConf.AuditoriaConf;
using Mre.Sb.AuditoriaConf.Domain;
using System;
using System.Collections.Generic;
using System.Text.Json;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Mre.Sb.AuditoriaConf.EntityFrameworkCore
{
    public static class AuditoriaConfDbContextModelCreatingExtensions
    {
        public static void ConfigureAuditoriaConf(
            this ModelBuilder builder,
            Action<AuditoriaConfModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new AuditoriaConfModelBuilderConfigurationOptions(
                AuditoriaConfDbProperties.DbTablePrefix,
                AuditoriaConfDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);



            builder.Entity<Auditable>(b =>
            { 
                b.ToTable(options.TablePrefix + "Auditable", options.Schema);

                b.HasIndex(x => new
                {
                    CategoriaId = x.CategoriaId,
                    Item = x.Item
                }).IsUnique();

                b.ConfigureByConvention();
            });

            builder.Entity<Categoria>(b =>
            {
                b.ToTable(options.TablePrefix + "CategoriaAuditable", options.Schema);

                b.ConfigureByConvention();
            });

            builder.Entity<Auditar>(b =>
            {
                b.ToTable(options.TablePrefix + "Auditar", options.Schema);

                b.Property(a => a.Acciones)
                .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null));

                b.ConfigureByConvention();
            });


        }
    }

}
