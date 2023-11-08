using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RequisicionesAlmacenBL.Entities;

namespace RequisicionesAlmacen.Areas.Almacenes.Reportes.Models.ViewModel
{
    public class ReporteExistenciasViewModel
    {
        public ARrptExistencias Reporte { get; set; }

        public IEnumerable<TipoReporteSelect> ListTiposReporte { get; set; }

        public IEnumerable<tblDependencia> ListUnidadesAdministrativas { get; set; }

        public IEnumerable<tblProyecto> ListProyectos { get; set; }

        public IEnumerable<tblRamo> ListFuentesFinanciamiento { get; set; }

        public IEnumerable<tblTipoGasto> ListTiposGasto { get; set; }

        public IEnumerable<tblObjetoGasto> ListObjetosGasto { get; set; }

        public class TipoReporteSelect
        {
            public int Id { get; set; }

            public string Value { get; set; }
        }
    }
}