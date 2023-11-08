using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RequisicionesAlmacen.Areas.Compras.Models
{
    public class DependenciaItem : tblDependencia
    {
        public string Id { get => DependenciaId; }

        public string Item { get => DependenciaId + " - " + Nombre; }

        public static DependenciaItem ConvertTo(tblDependencia obj)
        {
            DependenciaItem item = new DependenciaItem();
            item.DependenciaId = obj.DependenciaId;
            item.Nombre = obj.Nombre;

            return item;
        }
    }
}