using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(ARrptReciboOCMetaData))]
    public partial class ARrptReciboOC
    {
        public static List<string> PropiedadesNoActualizables = new List<string>();               

        //Columnas Reporte
        public string FechaOC { get; set; }
        
        public string FechaRecibo { get; set; }

        //Filtros
        public Nullable<DateTime> FechaInicio { get; set; }

        public Nullable<DateTime> FechaFin { get; set; }

        public Nullable<int> OrdenCompraId { get; set; }

        public Nullable<int> CompraId { get; set; }
    }

    public class ARrptReciboOCMetaData
    {
        [Display(Name = "Fecha y Hora Orden de Compra")]
        public string FechaOC { get; set; }

        [Display(Name = "Fecha y Hora Recibo")]
        public string FechaRecibo { get; set; }

        [Display(Name = "Fecha Inicio")]
        public Nullable<DateTime> FechaInicio { get; set; }

        [Display(Name = "Fecha Fin")]
        public Nullable<DateTime> FechaFin { get; set; }

        [Display(Name = "Código Orden de Compra")]
        public Nullable<int> OrdenCompraId { get; set; }

        [Display(Name = "Código Recibo")]
        public Nullable<int> CompraId { get; set; }
    }
}