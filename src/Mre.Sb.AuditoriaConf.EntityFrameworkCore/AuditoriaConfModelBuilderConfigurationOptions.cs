using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Mre.Sb.AuditoriaConf.EntityFrameworkCore
{
    public class AuditoriaConfModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public AuditoriaConfModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }

}
