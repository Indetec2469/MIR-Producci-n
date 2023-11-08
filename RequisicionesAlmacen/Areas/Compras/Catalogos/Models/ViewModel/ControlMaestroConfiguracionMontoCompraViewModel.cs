using RequisicionesAlmacenBL.Entities;
using System.Collections.Generic;

namespace RequisicionesAlmacen.Areas.Compras.Catalogos.Models.ViewModel
{
    public class ControlMaestroConfiguracionMontoCompraViewModel
    {
        public ARtblControlMaestroConfiguracionMontoCompra ControlMaestroConfiguracionMontoCompra { get; set; }
        public IEnumerable<ARtblControlMaestroConfiguracionMontoCompra> ListConfiguracionMontoCompra { get; set; }
        public IEnumerable<GRtblControlMaestro> ListTipoCompra { get; set; }

    }
}