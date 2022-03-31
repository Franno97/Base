using AutoMapper;
using Mre.Sb.AuditoriaConf.AuditoriaConf;
using Volo.Abp.AutoMapper;

namespace Mre.Sb.AuditoriaConf.Application
{
    public class AuditoriaConfApplicationAutoMapperProfile : Profile
    {
        public AuditoriaConfApplicationAutoMapperProfile()
        {
            CreateMap<Categoria, CategoriaDto>() 
               ;
           
            CreateMap<CategoriaDto, Categoria>()
                ;
             


            CreateMap<CrearActualizarAuditableDto, Auditable>()
              .ForMember(x => x.Id, map => map.Ignore())
              .ForMember(x => x.Categoria, map => map.Ignore())
              //.IgnoreAuditedObjectProperties()
              ;
             
        }
    }
}
