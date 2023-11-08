using RequisicionesAlmacen.Areas.Compras.Models;
using RequisicionesAlmacen.Areas.RH.Ingreso.Models;
using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.Almacenes.Almacenes.Models.ViewModel
{
    public class InventarioAjusteViewModel
    {
        public ARtblInventarioAjuste InventarioAjuste { get; set; }

        public InventarioAjusteDetalleItem InventarioAjusteDetalleItem { get; set; }

        public IEnumerable<ARvwListadoInventarioAjuste> ListInventarioAjuste { get; set; }

        public IEnumerable<tblAlmacen> ListAlmacenes { get; set; }

        public IEnumerable<ProductoItem> ListProductos { get; set; }

        public IEnumerable<GRtblControlMaestro> ListTiposMovimiento { get; set; }        

        public IEnumerable<ARtblControlMaestroConceptoAjusteInventario> ListConceptosAjuste { get; set; }

        public IEnumerable<InventarioAjusteDetalleItem> ListInventarioAjusteDetallesItems { get; set; }

        public Nullable<DateTime> FechaOperacion { get; set; }
    }
}