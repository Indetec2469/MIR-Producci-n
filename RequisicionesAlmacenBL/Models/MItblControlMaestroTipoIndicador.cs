using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(MItblControlMaestroTipoIndicadorMetaData))]
    public partial class MItblControlMaestroTipoIndicador
    {
        public static List<string> PropiedadesNoActualizables = new List<string>();

    }
    public class MItblControlMaestroTipoIndicadorMetaData
    {
        [Display(Name = "Código")]
        public int TipoIndicadorId { get; set; }

        [Required(ErrorMessage = "Indicador requerido")]
        [Display(Name = "Indicador")]
        public string Descripcion { get; set; }
    }
}
