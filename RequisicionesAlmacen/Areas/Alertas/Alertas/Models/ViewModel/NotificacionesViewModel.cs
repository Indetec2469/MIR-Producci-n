using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.Alertas.Alertas.Models.ViewModel
{
    public class NotificacionesViewModel
    {
        public List<GRspGetListadoAlertasPorUsuario_Result> ListAlertas { get; set; }
    }
}