using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.Compras.Compras.Models.ViewModel
{
    public class OrdenCompraViewModel
    {
        public tblOrdenCompra OrdenCompra { get; set; }
        
        public IEnumerable<ARvwListadoOrdenCompra> ListOrdenCompra { get; set; }
        
        public IEnumerable<ARspConsultaOrdenCompraDetalles_Result> ListOrdenCompraDetalles { get; set; }

        public IEnumerable<tblProveedor> ListProveedores { get; set; }

        public IEnumerable<tblAlmacen> ListAlmacenes { get; set; }

        public IEnumerable<tblTipoOperacion> ListTiposOperacion { get; set; }

        public IEnumerable<tblTipoComprobanteFiscal> ListTiposComprobanteFiscal { get; set; }

        public IEnumerable<ARspConsultaOrdenCompraProductos_Result> ListProductos { get; set; }

        public IEnumerable<tblDependencia> ListUnidadesAdministrativas { get; set; }

        public IEnumerable<tblProyecto> ListProyectos { get; set; }

        public IEnumerable<tblRamo> ListFuentesFinanciamiento { get; set; }

        public IEnumerable<tblTipoGasto> ListTiposGasto { get; set; }

        public IEnumerable<tblTarifaImpuesto> ListTarifasImpuesto { get; set; }

        public bool SoloLectura { get; set ; } = false;

        public string EjercicioUsuario { get; set; }

        public string Poliza { get; set; }

        public string PolizaCancelacion { get; set; }

        public Nullable<DateTime> FechaOperacion { get; set; }
    }
}