using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mre.Sb.Base.Data;
using Volo.Abp.DependencyInjection;

namespace Mre.Sb.Base.EntityFrameworkCore
{
    public class EntityFrameworkCoreBaseDbSchemaMigrator
        : IBaseDbSchemaMigrator, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public EntityFrameworkCoreBaseDbSchemaMigrator(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task MigrateAsync()
        {
            /* We intentionally resolving the BaseDbContext
             * from IServiceProvider (instead of directly injecting it)
             * to properly get the connection string of the current tenant in the
             * current scope.
             */

            await _serviceProvider
                .GetRequiredService<BaseDbContext>()
                .Database
                .MigrateAsync();
        }
    }
}
