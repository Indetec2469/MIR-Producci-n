using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RequisicionesAlmacen.Areas.Compras.Models
{
    public class ProyectoItem : tblProyecto
    {
        public string Id { get => ProyectoId; }

        public string Item { get => ProyectoId + " - " + Nombre; }

        public static ProyectoItem ConvertTo(tblProyecto obj)
        {
            ProyectoItem item = new ProyectoItem();
            item.ProyectoId = obj.ProyectoId;
            item.Nombre = obj.Nombre;

            return item;
        }
    }
}