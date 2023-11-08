using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RequisicionesAlmacenBL.Entities
{
    public class ProductoItem : tblProducto
    {
        public string Id { get => ProductoId; }

        public string Item { get => ProductoId + " - " + Descripcion; }

        public static ProductoItem ConvertTo(tblProducto obj)
        {
            ProductoItem item = new ProductoItem();
            item.ProductoId = obj.ProductoId;
            item.Descripcion = obj.Descripcion;

            return item;
        }
    }
}