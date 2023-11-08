using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(ARtblInventarioAjusteMetaData))]
    public partial class ARtblInventarioAjuste
    {
        public static List<string> PropiedadesNoActualizables = new List<string>();

    }

    public class ARtblInventarioAjusteMetaData
    {
        [Display(Name = "Código")]
        public string CodigoAjusteInventario { get; set; }

        [Display(Name = "Total ajuste")]
        public decimal MontoAjuste { get; set; }
    }
}
