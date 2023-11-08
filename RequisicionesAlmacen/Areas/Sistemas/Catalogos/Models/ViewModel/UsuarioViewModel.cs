using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RequisicionesAlmacen.Areas.Sistemas.Catalogos.Models.ViewModel
{
    public class UsuarioViewModel
    {
        public IEnumerable<GRvwListadoUsuario> ListadoUsuario { get; set; }
        public GRtblUsuario Usuario { get; set; }
        public IEnumerable<GRtblRol> ListaRol { get; set; }
        public IEnumerable<RHspConsultaEmpleadoLigado_Result> ListaEmpleado { get; set; }
        public IEnumerable<GRspArbolPermisoFicha_Result> ListaArbolPermisoFicha { get; set; }
        public IEnumerable<GRtblUsarioPermiso> ListaUsuarioPermiso { get; set; }
        public GRtblUsarioPermiso UsuarioPermisoModel { get; set; }
    }
}