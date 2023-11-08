using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.Sistemas.Sistema.Models.ViewModel
{
    public class ConfiguracionViewModel
    {
        public GRtblConfiguracionEnte ConfiguracionEnte { get; set; }

        public IEnumerable<GRtblControlMaestro> ListTipoConfiguracionFecha { get; set; }

        public string EjercicioUsuario { get; set; }
    }
}