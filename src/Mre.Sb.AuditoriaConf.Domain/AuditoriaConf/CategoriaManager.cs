using Microsoft.Extensions.Localization;
using Mre.Sb.AuditoriaConf.Localization;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
namespace Mre.Sb.AuditoriaConf.AuditoriaConf
{
    public class CategoriaManager : DomainService
    {
        private readonly IStringLocalizer<AuditoriaConfResource> localizer;

        private readonly IRepository<Categoria, string> repository;


        public CategoriaManager(IRepository<Categoria, string> repository,
            IStringLocalizer<AuditoriaConfResource> localizer)
        {
            this.repository = repository;
            this.localizer = localizer;

        }

        public async Task ValidateCreateAsync(string input)
        {

            var exist = await repository.AnyAsync(e => e.Id.ToUpper() == input.ToUpper());
            if (exist)
            {
                var msg = string.Format(localizer["Categoria:Exist"], input);
                throw new UserFriendlyException(msg);
            }
        }
    }


}
