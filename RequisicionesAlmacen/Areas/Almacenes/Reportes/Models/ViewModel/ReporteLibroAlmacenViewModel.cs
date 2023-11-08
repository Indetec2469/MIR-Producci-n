using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RequisicionesAlmacenBL.Entities;

namespace RequisicionesAlmacen.Areas.Almacenes.Reportes.Models.ViewModel
{
    public class ReporteLibroAlmacenViewModel
    {
        public ARrptLibroAlmacen Reporte { get; set; }

        public IEnumerable<TipoReporteSelect> ListTiposReporte { get; set; }

        public IEnumerable<tblAlmacen> ListAlmacenes { get; set; }

        public IEnumerable<CGvwClasificadorObjetoGasto> ListClasificadorObjetoGasto { get; set; }

        public Nullable<DateTime> FechaOperacion { get; set; }

        public class TipoReporteSelect
        {
            public int Id { get; set; }

            public string Value { get; set; }
        }
    }
}