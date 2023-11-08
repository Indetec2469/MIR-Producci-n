using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(MItblPlanDesarrolloEstructuraMetaData))]
    public partial class MItblPlanDesarrolloEstructura
    {
        public static List<string> PropiedadesNoActualizables = new List<string>();

    }

    public class MItblPlanDesarrolloEstructuraMetaData
    {
        [Display(Name = "Estructura Padre")]
        //[Required(ErrorMessage = "Estructura Padre requerida")]
        public int EstructuraPadreId { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Nombre requerido")]
        public string Nombre { get; set; }

        [Display(Name = "Etiqueta")]
        [Required(ErrorMessage = "Etiqueta requerida")]
        public string Etiqueta { get; set; }
    }
}
