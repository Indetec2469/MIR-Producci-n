using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RequisicionesAlmacen.Areas.MIR.Catalogos.Models.ViewModel
{
    public class ControlMaestroFrecuenciaMedicionViewModel
    {
        public IEnumerable<MItblControlMaestroFrecuenciaMedicion> ListControlMaestroFrecuenciaMedicion { get; set; }
        public IEnumerable<MItblControlMaestroFrecuenciaMedicionNivel> ListControlMaestroFrecuenciaMedicionNivel { get; set; }
        public IEnumerable<GRtblControlMaestro> ListControlMaestroNivel { get; set; }
    }
}