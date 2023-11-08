using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(ARtblInventarioFisicoMetaData))]
    public partial class ARtblInventarioFisico
    {
        public static List<string> PropiedadesNoActualizables = new List<string>();

    }

    public class ARtblInventarioFisicoMetaData
    {
        [Display(Name = "Código")]
        public string Codigo { get; set; }

        [Display(Name = "Almacén")]
        public string AlmacenId { get; set; }

        [Display(Name = "Total ajuste")]
        public decimal MontoAjuste { get; set; }
    }
}
