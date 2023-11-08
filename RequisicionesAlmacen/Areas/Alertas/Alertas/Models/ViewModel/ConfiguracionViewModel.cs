using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.Alertas.Alertas.Models.ViewModel
{
    public class ConfiguracionViewModel
    {
        public IEnumerable<GRspAlertasConfiguracionMenuPrincipal_Result> ListMenuPrincipal { get; set; }

        public IEnumerable<GRtblControlMaestro> ListTipoNotificacion { get; set; }

        public IEnumerable<GRspAlertasConfiguracionEmpleados_Result> ListEmpleado { get; set; }

        public IEnumerable<GRtblAlertaConfiguracion> ListAlertaConfiguracion { get; set; }
    }
}