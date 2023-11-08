using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(GRtblConfiguracionEnteMetaData))]
    public partial class GRtblConfiguracionEnte
    {
        public static List<string> PropiedadesNoActualizables = new List<string>() {
            "CreadoPorId",
            "FechaCreacion"
        };
    }

    public class GRtblConfiguracionEnteMetaData
    {
        [Display(Name = "Tipo Configuración de Fecha")]
        [Required(ErrorMessage = "Tipo Configuración de Fecha requerida")] 
        public int TipoConfiguracionFechaId { get; set; }

        [Display(Name = "Fecha de Operación")] 
        public Nullable<DateTime> FechaOperacion { get; set; }
    }
}