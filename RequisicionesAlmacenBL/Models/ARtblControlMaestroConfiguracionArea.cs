using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(ARtblControlMaestroConfiguracionAreaMetaData))]
    public partial class ARtblControlMaestroConfiguracionArea
    {
        public List<string> PropiedadesNoActualizables = new List<string>();
    }

    public class ARtblControlMaestroConfiguracionAreaMetaData
    {
        [Display(Name = "Área solicitante")]
        [Required(ErrorMessage = "Área solicitante requerida")]        
        public string AreaId { get; set; }

        [Display(Name = "Comentarios")]
        public string Comentarios { get; set; }
    }

    [MetadataType(typeof(ARvwListadoConfiguracionArea))]
    public partial class ARvwListadoConfiguracionArea
    {

    }

    public class ARvwListadoConfiguracionAreaMetaData
    {
        [Display(Name = "Código")]
        public string Codigo { get; set; }

        [Display(Name = "Área solicitante")]
        public string Area { get; set; }
    }
}
