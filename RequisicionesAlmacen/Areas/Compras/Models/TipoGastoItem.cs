using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RequisicionesAlmacen.Areas.Compras.Models
{
    public class TipoGastoItem : tblTipoGasto
    {
        public string Id { get => TipoGastoId; }

        public string Item { get => TipoGastoId + " - " + Nombre; }

        public static TipoGastoItem ConvertTo(tblTipoGasto obj)
        {
            TipoGastoItem item = new TipoGastoItem();
            item.TipoGastoId = obj.TipoGastoId;
            item.Nombre = obj.Nombre;

            return item;
        }
    }
}