using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(ARrptSurtidoMaterialesMetaData))]
    public partial class ARrptSurtidoMateriales
    {
        public static List<string> PropiedadesNoActualizables = new List<string>();               

        //Columnas Reporte
        public int InventarioMovtoAgrupadorId { get; set; }

        public int RequisicionMaterialId { get; set; }

        public DateTime Fecha { get; set; }

        //Filtros
        public Nullable<DateTime> FechaInicio { get; set; }

        public Nullable<DateTime> FechaFin { get; set; }

        public string Solicitud { get; set; }

        public string Poliza { get; set; }
    }

    public class ARrptSurtidoMaterialesMetaData
    {
        [Display(Name = "Id")]
        public int InventarioMovtoAgrupadorId { get; set; }

        [Display(Name = "Fecha y Hora")]
        public DateTime Fecha { get; set; }

        [Display(Name = "Fecha Inicio")]
        public Nullable<DateTime> FechaInicio { get; set; }

        [Display(Name = "Fecha Fin")]
        public Nullable<DateTime> FechaFin { get; set; }

        [Display(Name = "Código Solicitud")]
        public string Solicitud { get; set; }

        [Display(Name = "Póliza")]
        public string Poliza { get; set; }
    }
}