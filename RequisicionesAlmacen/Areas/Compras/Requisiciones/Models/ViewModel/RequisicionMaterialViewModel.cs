using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.Compras.Requisiciones.Models.ViewModel
{
    public class RequisicionMaterialViewModel
    {
        public ARtblRequisicionMaterial RequisicionMaterial { get; set; }
        
        public IEnumerable<ARvwListadoRequisicionMaterial> ListRequisicionMaterial { get; set; }
        
        public IEnumerable<RequisicionMaterialDetalleItem> ListRequisicionMaterialDetalles { get; set; }

        public RHtblEmpleado Empleado { get; set; }

        public IEnumerable<RHtblEmpleado> ListEmpleados { get; set; }

        public IEnumerable<tblDependencia> ListAreas { get; set; }

        public IEnumerable<tblDependencia> ListUnidadesAdministrativas { get; set; }
        
        public IEnumerable<tblProyecto> ListProyectos { get; set; }
        
        public IEnumerable<tblProyecto_Dependencia> ListProyectosDependencias { get; set; }

        public IEnumerable<tblUnidadDeMedida> ListUnidadesMedida { get; set; }

        public IEnumerable<ARspConsultaRequisicionMaterialProductos_Result> ListProductos { get; set; }

        public string AreaEmpleadoId { get; set; }

        public bool SoloLectura { get; set ; } = false;
        
        public string EjercicioUsuario { get; set; }

        public Nullable<DateTime> FechaOperacion { get; set; }

        public bool PermisoTerceros { get; set; } = false;
    }
}