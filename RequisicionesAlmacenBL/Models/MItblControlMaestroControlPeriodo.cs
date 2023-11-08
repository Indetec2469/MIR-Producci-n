using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(MItblControlMaestroControlPeriodoMetaData))]
    public partial class MItblControlMaestroControlPeriodo
    {
        public static List<string> PropiedadesNoActualizables = new List<string>();

    }

    public class MItblControlMaestroControlPeriodoMetaData
    {
        [Display(Name = "Código")]
        public string Codigo { get; set; }

        [Display(Name = "Mes")]
        public string Periodo { get; set; }

        [Display(Name = "Estatus")]
        public int EstatusPeriodoId { get; set; }
    }
}
