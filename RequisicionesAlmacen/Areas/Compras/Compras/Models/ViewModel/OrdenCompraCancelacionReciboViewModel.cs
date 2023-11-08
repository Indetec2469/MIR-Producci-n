using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.Compras.Compras.Models.ViewModel
{
    public class OrdenCompraCancelacionReciboViewModel
    {
        public tblCompra Compra { get; set; }
        
        public List<ARvwListadoOrdenCompraRecibo> ListOrdenCompraRecibo { get; set; }
        
        public List<OrdenCompraReciboDetalleItem> ListOrdenCompraReciboDetalles { get; set; }

        public List<ARspConsultaOCPorRecibir_Result> ListOrdenesCompra { get; set; }

        public List<ARspConsultaOCPorRecibirDetalles_Result> ListOrdenesCompraDetalles { get; set; }

        public IEnumerable<tblProveedor> ListProveedores { get; set; }

        public IEnumerable<tblAlmacen> ListAlmacenes { get; set; }

        public IEnumerable<tblTarifaImpuesto> ListTarifasImpuesto { get; set; }

        public bool SoloLectura { get; set ; } = false;

        public string EjercicioUsuario { get; set; }
    }
}