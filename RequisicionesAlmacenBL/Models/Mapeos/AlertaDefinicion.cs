using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequisicionesAlmacenBL.Models.Mapeos
{
    public class AlertaDefinicion
    {
        public static int REQUISICION_MATERIA_AUTORIZACION = 1;
        public static int REQUISICION_MATERIA_AUTORIZADA = 2;
        public static int REQUISICION_MATERIA_RECHAZADA = 3;
        public static int REQUISICION_MATERIA_EN_REVISION = 4;
    }
}