using AutoMapper;
using Mre.Sb.Base.Permiso;
using Volo.Abp.PermissionManagement;

namespace Mre.Sb.Base
{
    public class BaseApplicationAutoMapperProfile : Profile
    {
        public BaseApplicationAutoMapperProfile()
        {
            

            CreateMap<PermissionGrant, PermissionGrantEto>(); 
        }
    }
}
