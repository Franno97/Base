using Microsoft.EntityFrameworkCore;
using Mre.Sb.AuditoriaConf.AuditoriaConf;
using Mre.Sb.AuditoriaConf.Domain;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Mre.Sb.AuditoriaConf.EntityFrameworkCore
{
    [ConnectionStringName(AuditoriaConfDbProperties.ConnectionStringName)]
    public interface IAuditoriaConfDbContext : IEfCoreDbContext
    {
        DbSet<Auditable> Auditable { get; set; }

          
        DbSet<Categoria> Categoria { get; set; }

          
        DbSet<Auditar> Auditar { get; set; }
    }

}
