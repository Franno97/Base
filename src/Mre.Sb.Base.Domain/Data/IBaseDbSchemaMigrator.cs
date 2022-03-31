using System.Threading.Tasks;

namespace Mre.Sb.Base.Data
{
    public interface IBaseDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}
