using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(GRvwListadoRolMetaData))]
    public partial class GRvwListadoRol
    {
    }

    public class GRvwListadoRolMetaData
    {
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
    }
}
