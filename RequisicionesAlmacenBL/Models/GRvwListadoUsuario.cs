using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(GRvwListadoUsuarioMetaData))]
    public partial class GRvwListadoUsuario
    {
    }

    public class GRvwListadoUsuarioMetaData
    {
        [Display(Name = "Nombre Usuario")]
        public string NombreUsuario { get; set; }

        [Display(Name = "Nombre Empleado")]
        public string NombreEmpleado { get; set; }

        [Display(Name = "Rol")]
        public string NombreRol { get; set; }
    }
}
