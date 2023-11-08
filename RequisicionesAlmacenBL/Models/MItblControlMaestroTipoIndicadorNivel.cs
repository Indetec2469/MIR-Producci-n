using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(MItblControlMaestroTipoIndicadorNivelMetaData))]
    public partial class MItblControlMaestroTipoIndicadorNivel
    {
        public static List<string> PropiedadesNoActualizables = new List<string>();

    }

    public class MItblControlMaestroTipoIndicadorNivelMetaData
    {
       
    }
}
