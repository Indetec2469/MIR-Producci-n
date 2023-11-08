using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RequisicionesAlmacen.Areas.Compras.Catalogos.Models.ViewModel
{
    public class RequisicionMaterialViewModel
    {
        public RequisicionMaterial RequisicionMaterial { get; set; }
        public IEnumerable<RequisicionMaterial> ListRequisicionMaterial { get; set; }
        public IEnumerable<RequisicionMaterialDetalle> ListRequisicionMaterialDetalle { get; set; }
        public IEnumerable<Usuario> ListUsuario { get; set; }
        public IEnumerable<EmpleadoSAACG> ListEmpleado { get; set; }
        public IEnumerable<RequisicionesAlmacen.Areas.Compras.Catalogos.Controllers.RequisicionMaterialController.Articulo> ListArticulo { get; set; }
        public IEnumerable<Producto> ListProducto { get; set; }
        public IEnumerable<Almacen> ListAlmacen { get; set; }
        public IEnumerable<UnidadDeMedida> ListUnidadDeMedida { get; set; }
        public IEnumerable<Proyecto> ListProyecto { get; set; }
        public IEnumerable<Ramo> ListRamo { get; set; }
    }

}