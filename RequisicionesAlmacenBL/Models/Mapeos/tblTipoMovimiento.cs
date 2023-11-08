using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequisicionesAlmacenBL.Models.Mapeos
{
    public class tblTipoMovimiento
    {
        public static int ORDEN_COMPRA = 1;
        public static int COMPRA = 2;
        public static int AJUSTE_INVENTARIO = 73;
        public static int TRANSFERENCIA_PRODUCTO = 74;
        public static int SURTIMIENTO_REQUISICION = 75;
    }
}