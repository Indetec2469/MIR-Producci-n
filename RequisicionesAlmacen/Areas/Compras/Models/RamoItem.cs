using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RequisicionesAlmacen.Areas.Compras.Models
{
    public class RamoItem : tblRamo
    {
        public string Id { get => RamoId; }

        public string Item { get => RamoId + " - " + Nombre; }

        public static RamoItem ConvertTo(tblRamo obj)
        {
            RamoItem item = new RamoItem();
            item.RamoId = obj.RamoId;
            item.Nombre = obj.Nombre;

            return item;
        }
    }
}