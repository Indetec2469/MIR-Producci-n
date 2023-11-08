using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.RH.Ingreso.Models.ViewModel
{
    public class EmpleadoViewModel
    {
        public RHtblEmpleado Empleado { get; set; }

        public IEnumerable ListTipoEmpleado { get; set; }

        public IEnumerable<tblDependencia> ListAreaAdscripcion { get; set; }

        public IEnumerable ListPuesto { get; set; }

        public IEnumerable ListCargo { get; set; }

        public IEnumerable<RHtblEmpleado> ListEmpleado { get; set; }

        public string ImageSrc { get; set; }
    }
}