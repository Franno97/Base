using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Mre.Sb.AuditoriaConf.AuditoriaConf;
using Mre.Sb.AuditoriaConf.Domain;
using System.IO;
using Volo.Abp.EntityFrameworkCore;

namespace Mre.Sb.AuditoriaConf.EntityFrameworkCore
{
    /* This class is needed for EF Core console commands
    * (like Add-Migration and Update-Database commands) */
    public class AuditoriaConfDbContextFactory : IDesignTimeDbContextFactory<AuditoriaConfDbContext>
    {
        public AuditoriaConfDbContext CreateDbContext(string[] args)
        {
        
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<AuditoriaConfDbContext>()
                .UseSqlServer(configuration.GetConnectionString(AuditoriaConfDbProperties.ConnectionStringName));

            return new AuditoriaConfDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Mre.Sb.Base.DbMigrator/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }

}
