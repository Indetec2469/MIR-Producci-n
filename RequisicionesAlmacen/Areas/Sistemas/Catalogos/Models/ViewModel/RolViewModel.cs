using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RequisicionesAlmacen.Areas.Sistemas.Catalogos.Models.ViewModel
{
    public class RolViewModel
    {
        public IEnumerable<GRvwListadoRol> ListadoRol { get; set; }
        public GRtblRol Rol { get; set; }
        public IEnumerable<GRtblRolMenu> ListRolMenu { get; set; }
        public GRtblRolMenu RolMenuModel { get; set; }
        public IEnumerable<GRtblMenuPrincipal> ListMenuPrincipal { get; set; }
    }
}