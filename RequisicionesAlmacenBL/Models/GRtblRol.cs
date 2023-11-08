using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(GRtblRolMetaData))]
    public partial class GRtblRol
    {
        public static List<string> PropiedadesNoActualizables = new List<string>();
    }

    public class GRtblRolMetaData
    {
        [Required(ErrorMessage = "Nombre requerido")]
        [StringLength(50, ErrorMessage = "La máxima de 50 caracteres")]
        public string Nombre { get; set; }

        [Display(Name = "Descripción")]
        [StringLength(100, ErrorMessage = "La máxima de 100 caracteres")]
        public string Descripcion { get; set; }
    }
}
