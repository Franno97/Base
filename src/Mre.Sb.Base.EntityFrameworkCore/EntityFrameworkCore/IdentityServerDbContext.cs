using Audit.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;

namespace Mre.Sb.Base.EntityFrameworkCore
{
    [ConnectionStringName("AbpIdentityServer")]
    public class IdentityServerDbContext :
        AbpDbContext<IdentityServerDbContext>
    {

        public IdentityServerDbContext(DbContextOptions<IdentityServerDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureIdentityServer();

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{

        //    optionsBuilder.AddInterceptors(new AuditSaveChangesInterceptor());
        //}
    }
}
