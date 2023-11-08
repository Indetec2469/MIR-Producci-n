using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RequisicionesAlmacenBL.Entities;

namespace RequisicionesAlmacen.Areas.Almacenes.Reportes.Models.ViewModel
{
    public class ReporteSurtidoMaterialesViewModel
    {
        public ARrptSurtidoMateriales Reporte { get; set; }

        public Nullable<DateTime> FechaInicio { get; set; }

        public Nullable<DateTime> FechaFin { get; set; }
    }
}