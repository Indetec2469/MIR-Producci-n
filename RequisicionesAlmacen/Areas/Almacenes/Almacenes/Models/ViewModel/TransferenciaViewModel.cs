using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.Almacenes.Almacenes.Models.ViewModel
{
    public class TransferenciaViewModel
    {
        public ARtblTransferencia Transferencia { get; set; }
        
        public IEnumerable<ARvwListadoTransferencias> ListTransferecias { get; set; }
        
        public IEnumerable<ARspConsultaTransferenciaDetalles_Result> ListTransferenciaMovtos { get; set; }

        public IEnumerable<ARspConsultaConfiguracionAreaAlmacenes_Result> ListAlmacenes { get; set; }

        public IEnumerable<ARspConsultaTransferenciaProductos_Result> ListProductos { get; set; }

        public bool SoloLectura { get; set ; } = false;

        public string EjercicioUsuario { get; set; }

        public Nullable<DateTime> FechaOperacion { get; set; }
    }
}