using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.Compras.Compras.Models.ViewModel
{
    public class CortesiaViewModel
    {
        public ARtblCortesia Cortesia { get; set; }
        
        public IEnumerable<ARvwListadoReciboCortesia> ListReciboCortesia { get; set; }
        
        public IEnumerable<ARspConsultaCortesiaDetalles_Result> ListCortesiaDetalles { get; set; }

        public IEnumerable<tblOrdenCompra> ListOrdenesCompra { get; set; }

        public IEnumerable<tblProveedor> ListProveedores { get; set; }

        public IEnumerable<tblAlmacen> ListAlmacenes { get; set; }

        public IEnumerable<ARspConsultaOrdenCompraProductos_Result> ListProductos { get; set; }

        public bool SoloLectura { get; set ; } = false;

        public string EjercicioUsuario { get; set; }
    }
}