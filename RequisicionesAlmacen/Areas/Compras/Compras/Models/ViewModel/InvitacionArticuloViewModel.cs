using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.Compras.Compras.Models.ViewModel
{
    public class InvitacionArticuloViewModel
    {
        public IEnumerable<ARspConsultaInvitacionArticulosPorConvertir_Result> ListArticulosInvitacion { get; set; }
    }
}