using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace Mre.Sb.Base.Identidad
{
    public interface IIdentidadConfiguracionAppService
    {

        Task<IdentidadConfiguracionDto> ObtenerAsync();

        Task ActualizarAsync(ActualizarIdentidadConfiguracionDtoDto input);

    }

    public class IdentidadConfiguracionDto {

        public int ClaveLongitud { get; set; }

        public bool ClaveRequiereMayusculas { get; set; }

        public bool ClaveRequiereMinusculas { get; set; }

        public bool ClaveRequiereDigito { get; set; }

        public bool ClaveRequiereNoAlfanumericos { get; set; }


        public int BloqueoMaximoAccesoFallidos { get; set; }

        public int BloqueoTiempo { get; set; }

        public bool BloqueoNuevosUsuarios { get; set; }

        public bool AccesoNotificarFallidos { get; set; }


        public bool ControlarClavesAnterior { get; set; }

      
        public int ControlarClavesAnteriorCantidad { get; set; }
    }

    public class ActualizarIdentidadConfiguracionDtoDto {

        [Range(1, 256)]
        public int ClaveLongitud { get; set; }


        public bool ClaveRequiereMayusculas { get; set; }

        public bool ClaveRequiereMinusculas { get; set; }

        public bool ClaveRequiereDigito { get; set; }

        public bool ClaveRequiereNoAlfanumericos { get; set; }


        [Range(0, 30)]
        public int BloqueoMaximoAccesoFallidos { get; set; }

        public int BloqueoTiempo { get; set; }

        public bool BloqueoNuevosUsuarios { get; set; }

        public bool AccesoNotificarFallidos { get; set; }


        public bool ControlarClavesAnterior { get; set; }


        [Range(0, 15)]
        public int ControlarClavesAnteriorCantidad { get; set; }


    }



}
