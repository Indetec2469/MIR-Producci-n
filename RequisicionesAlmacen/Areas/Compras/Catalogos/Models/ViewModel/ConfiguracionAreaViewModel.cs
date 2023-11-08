using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.Compras.Catalogos.Models.ViewModel
{
    public class ConfiguracionAreaViewModel
    {
        public IEnumerable<ARvwListadoConfiguracionArea> ListConfiguracionArea { get; set; }

        public IEnumerable<tblDependencia> ListAreas { get; set; }

        public IEnumerable<ARspConsultaConfiguracionAreaProyectos_Result> ListDependenciasProyectos { get; set; }

        public IEnumerable<ARspConsultaConfiguracionAreaAlmacenes_Result> ListAlmacenes { get; set; }

        public ARtblControlMaestroConfiguracionArea ConfiguracionArea { get; set; }

        public IEnumerable<ARtblControlMaestroConfiguracionAreaProyecto> ListConfiguracionAreaProyectos { get; set; }

        public IEnumerable<ARtblControlMaestroConfiguracionAreaAlmacen> ListConfiguracionAreaAlmacenes { get; set; }
    }
}