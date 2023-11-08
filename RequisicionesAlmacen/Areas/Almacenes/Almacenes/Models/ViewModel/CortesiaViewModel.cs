using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections.Generic;

namespace RequisicionesAlmacen.Areas.Almacenes.Almacenes.Models.ViewModel
{
    public class CortesiaViewModel
    {
        public ARtblCortesia Cortesia { get; set; }
        
        public IEnumerable<ARvwListadoReciboCortesia> ListReciboCortesia { get; set; }
        
        public IEnumerable<ARspConsultaCortesiaDetalles_Result> ListCortesiaDetalles { get; set; }

        public IEnumerable<tblOrdenCompra> ListOrdenesCompra { get; set; }

        public IEnumerable<tblProveedor> ListProveedores { get; set; }

        public IEnumerable<tblAlmacen> ListAlmacenes { get; set; }

        public IEnumerable<tblProducto> ListProductos { get; set; }

        public IEnumerable<ARspConsultaOrdenCompraProductos_Result> ListCuentasPresupuestales { get; set; }
        
        public bool SoloLectura { get; set ; } = false;

        public string EjercicioUsuario { get; set; }

        public Nullable<DateTime> FechaOperacion { get; set; }
    }
}