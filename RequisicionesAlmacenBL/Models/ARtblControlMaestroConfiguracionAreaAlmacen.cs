using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(ARtblControlMaestroConfiguracionAreaAlmacenMetaData))]
    public partial class ARtblControlMaestroConfiguracionAreaAlmacen
    {
        public List<string> PropiedadesNoActualizables = new List<string>();

    }

    public class ARtblControlMaestroConfiguracionAreaAlmacenMetaData
    {
        
    }

    [MetadataType(typeof(ListAlmacenesMetaData))]
    public partial class ListAlmacenes
    {
        public static List<string> PropiedadesNoActualizables = new List<string>();
    }

    public class ListAlmacenesMetaData
    {
        [Display(Name = "Almacén")]
        public string Nombre { get; set; }
    }
}