using RequisicionesAlmacen.Areas.Compras.Models;
using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections.Generic;

namespace RequisicionesAlmacen.Areas.Almacenes.Almacenes.Models.ViewModel
{
    public class InventarioFisicoViewModel
    {
        public ARtblInventarioFisico InventarioFisico { get; set; }

        public IEnumerable<ARvwListadoInventarioFisico> ListInventarioFisico { get; set; }

        public IEnumerable<tblAlmacen> ListAlmacenes { get; set; }

        public IEnumerable<ObjetoGastoItem> ListGrupos { get; set; }

        public IEnumerable<ObjetoGastoItem> ListSubGrupos { get; set; }

        public IEnumerable<ObjetoGastoItem> ListClases { get; set; }

        public IEnumerable<ObjetoGastoItem> ListPartidasEspecificas { get; set; }

        public IEnumerable<ProductoItem> ListProductos { get; set; }

        public IEnumerable<ExistenciaAlmacen> ListExistenciasAlmacen { get; set; }

        public Nullable<DateTime> FechaOperacion { get; set; }
    }
}