using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RequisicionesAlmacenBL.Entities;

namespace RequisicionesAlmacen.Areas.Almacenes.Reportes.Models.ViewModel
{
    public class ReporteKardexViewModel
    {
        public ARrptKardex Reporte { get; set; }

        public IEnumerable<tblAlmacen> ListAlmacenes { get; set; }

        public IEnumerable<tblProducto> ListProductos { get; set; }

        public IEnumerable<GRtblControlMaestro> ListTiposMovimiento { get; set; }

        public IEnumerable<tblDependencia> ListUnidadesAdministrativas { get; set; }

        public IEnumerable<tblProyecto> ListProyectos { get; set; }

        public IEnumerable<tblRamo> ListFuentesFinanciamiento { get; set; }

        public IEnumerable<tblPoliza> ListPolizas { get; set; }

        public Nullable<DateTime> FechaInicio { get; set; }

        public Nullable<DateTime> FechaFin { get; set; }
    }
}