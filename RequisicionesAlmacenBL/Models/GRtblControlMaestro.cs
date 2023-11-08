using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(GRtblControlMaestroMetaData))]
    public partial class GRtblControlMaestro
    {
        public static List<string> PropiedadesNoActualizables = new List<string>();

    }

    public class GRtblControlMaestroMetaData
    {
        public string Control { get; set; }
        public string Valor { get; set; }
        public bool Sistema { get; set; }
        public bool Activo { get; set; }
        public bool ControlSencillo { get; set; }
    }
}
