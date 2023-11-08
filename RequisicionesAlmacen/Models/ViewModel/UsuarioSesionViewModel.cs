using RequisicionesAlmacenBL.Entities;
using SACG.sysSacg.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RequisicionesAlmacen.Models.ViewModel
{
    public class UsuarioSesionViewModel
    {

        public UsuarioSesion UsuarioSesion { get; set; }
        public IEnumerable<Entidad> ListEntes { get; set; }
        public int TipoVersion { get; set; }

    }
}