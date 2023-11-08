using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RequisicionesAlmacenBL.Entities;

namespace RequisicionesAlmacen.Areas.Almacenes.Catalogos.Models.ViewModel
{
    public class ControlMaestroConceptoAjusteInventarioViewModel
    {
        public ARtblControlMaestroConceptoAjusteInventario ControlMaestroConceptoAjusteInventario { get; set; }
        
        public IEnumerable<ARtblControlMaestroConceptoAjusteInventario> ListControlMaestroConceptoAjusteInventario { get; set; }
        
        public IEnumerable<GRtblControlMaestro> ListTipoMovimiento { get; set; }
    }
}