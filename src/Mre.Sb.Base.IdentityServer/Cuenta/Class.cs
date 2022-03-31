//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Options;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Volo.Abp.BackgroundWorkers;
//using Volo.Abp.IdentityServer.Grants;
//using Volo.Abp.IdentityServer.Tokens;
//using Volo.Abp.Threading;
//using Volo.Abp.Timing;
//using Volo.Abp.Uow;

//namespace Mre.Sb.Base.Cuenta
//{
//    public class TokenCleanupBackgroundWorkerFoo : AsyncPeriodicBackgroundWorkerBase
//    {
//        protected TokenCleanupOptions Options { get; }
//        public AbpAsyncTimer Timer { get; }
//        public IServiceScopeFactory ServiceScopeFactory { get; }
//        public IOptions<TokenCleanupOptions> Options1 { get; }
//        public IPersistentGrantRepository PersistentGrantRepository { get; }
//        public IClock Clock { get; }

//        public TokenCleanupBackgroundWorkerFoo(
//            AbpAsyncTimer timer,
//            IServiceScopeFactory serviceScopeFactory,
//            IOptions<TokenCleanupOptions> options,
//            IPersistentGrantRepository persistentGrantRepository,
//            IClock clock)
//            : base(
//                timer,
//                serviceScopeFactory)
//        {
//            Options = options.Value;
//            timer.Period = 60000; //1. Minuto Options.CleanupPeriod;
//            Timer = timer;
//            ServiceScopeFactory = serviceScopeFactory;
//            Options1 = options;
//            PersistentGrantRepository = persistentGrantRepository;
//            Clock = clock;
//        }

//        protected async override Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
//        {
//            var x = workerContext
//                .ServiceProvider
//                .GetRequiredService<TokenCleanupService>();

//            var persistentGrants = await PersistentGrantRepository.GetListByExpirationAsync(DateTime.UtcNow, Options.CleanupBatchSize);

//            var persistentGrants2 = await PersistentGrantRepository
//               .GetListByExpirationAsync(Clock.Now, Options.CleanupBatchSize);


//            await x
//                .CleanAsync();


//            await RemoveGrantsAsync();
//        }


//        [UnitOfWork]
//        protected virtual async Task RemoveGrantsAsync()
//        {
//            for (var i = 0; i < Options.CleanupLoopCount; i++)
//            {
//                var persistentGrants = await PersistentGrantRepository.GetListByExpirationAsync(DateTime.UtcNow, Options.CleanupBatchSize);

//                await PersistentGrantRepository.DeleteManyAsync(persistentGrants);

//                //No need to continue to query if it gets more than max items.
//                if (persistentGrants.Count < Options.CleanupBatchSize)
//                {
//                    break;
//                }
//            }
//        }

//        [UnitOfWork]
//        protected virtual async Task RemoveGrantsAsync2()
//        {
//            for (int i = 0; i < Options.CleanupLoopCount; i++)
//            {
//                var persistentGrants = await PersistentGrantRepository
//                    .GetListByExpirationAsync(Clock.Now, Options.CleanupBatchSize);

//                //TODO: Can be optimized if the repository implements the batch deletion
//                foreach (var persistentGrant in persistentGrants)
//                {
//                    await PersistentGrantRepository
//                        .DeleteAsync(persistentGrant);
//                }

//                //No need to continue to query if it gets more than max items.
//                if (persistentGrants.Count < Options.CleanupBatchSize)
//                {
//                    break;
//                }
//            }
//        }
//    }
//}
