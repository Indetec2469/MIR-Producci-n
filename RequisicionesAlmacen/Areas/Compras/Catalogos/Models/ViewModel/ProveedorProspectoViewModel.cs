using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.Compras.Catalogos.Models.ViewModel
{
    public class ProveedorProspectoViewModel
    {
        public ARtblProveedorProspecto ProveedorProspecto { get; set; }

        public IEnumerable<ARtblProveedorProspecto> ListProveedorProspecto { get; set; }
        
        public IEnumerable<tblPais> ListPaises { get; set; }
        
        public IEnumerable<tblEstado> ListEstados { get; set; }
        
        public IEnumerable<tblMunicipio> ListMunicipios { get; set; }
        
        public IEnumerable<tblTipoProveedor> ListTiposProveedor { get; set; }
        
        public IEnumerable<tblTipoOperacion> ListTiposOperacion { get; set; }
        
        public IEnumerable<tblTipoComprobanteFiscal> ListTiposComprobanteFiscal { get; set; }
    }
}