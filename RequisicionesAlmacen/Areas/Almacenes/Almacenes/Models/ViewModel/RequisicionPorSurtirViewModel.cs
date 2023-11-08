using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.Almacenes.Almacenes.Models.ViewModel
{
    public class RequisicionPorSurtirViewModel
    {
        public ARtblRequisicionMaterial RequisicionMaterial { get; set; }

        public IEnumerable<ARspConsultaRequisicionPorSurtirDetalles_Result> ListRequisicionMaterialDetalles { get; set; }

        public IEnumerable<ARspConsultaRequisicionPorSurtirExistencias_Result> ListExistencias { get; set; }

        public string Solicitante { get; set; }

        public string Area { get; set; }

        public string Fecha { get; set; }

        public string Estatus { get; set; }

        public IEnumerable<ARvwListadoRequisicionPorSurtir> ListRequisicionPorSurtir { get; set; }

        public Nullable<DateTime> FechaOperacion { get; set; }
    }
}