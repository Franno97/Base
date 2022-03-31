using Microsoft.EntityFrameworkCore;
using Mre.Sb.AuditoriaConf.AuditoriaConf;
using Mre.Sb.AuditoriaConf.Domain;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Mre.Sb.AuditoriaConf.EntityFrameworkCore
{

    [ConnectionStringName(AuditoriaConfDbProperties.ConnectionStringName)]
    public class AuditoriaConfDbContext : AbpDbContext<AuditoriaConfDbContext>, IAuditoriaConfDbContext
    {
        public DbSet<Auditable> Auditable { get; set; }

        public DbSet<Categoria> Categoria { get; set; }

        public DbSet<Auditar> Auditar { get; set; }

        public AuditoriaConfDbContext(DbContextOptions<AuditoriaConfDbContext> options)
           : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureAuditoriaConf();
        }
    }

}
