using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(tblCompraDetMetaData))]
    public partial class tblCompraDet
    {
        public static List<string> PropiedadesNoActualizables = new List<string>();

        public int AlmacenProductoId { get; set; }
    }

    public class tblCompraDetMetaData
    {
        
    }
}