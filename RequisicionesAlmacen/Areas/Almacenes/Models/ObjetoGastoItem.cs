using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RequisicionesAlmacenBL.Entities
{
    public class ObjetoGastoItem : tblObjetoGasto
    {
        public string Id { get => ObjetoGastoId; }

        public string Item { get => ObjetoGastoId + " - " + Nombre; }

        public static ObjetoGastoItem ConvertTo(tblObjetoGasto obj)
        {
            ObjetoGastoItem item = new ObjetoGastoItem();
            item.ObjetoGastoId = obj.ObjetoGastoId;
            item.Nombre = obj.Nombre;

            return item;
        }
    }
}