using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(ARtblControlMaestroConfiguracionAreaProyectoMetaData))]
    public partial class ARtblControlMaestroConfiguracionAreaProyecto
    {
        public List<string> PropiedadesNoActualizables = new List<string>();

    }

    public class ARtblControlMaestroConfiguracionAreaProyectoMetaData
    {
        
    }

    [MetadataType(typeof(ListDependenciasProyectosMetaData))]
    public partial class ListDependenciasProyectos
    {
        public static List<string> PropiedadesNoActualizables = new List<string>();
    }

    public class ListDependenciasProyectosMetaData
    {
        [Display(Name = "Unidad Administrativa y Proyecto")]
        public string Nombre { get; set; }
    }
}