using RequisicionesAlmacenBL.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RequisicionesAlmacen.Areas.MIR.Catalogos.Models.ViewModel
{
    public class PlanDesarrolloViewModel
    {
        public IEnumerable<MIvwListadoPlanNacionalDesarrollo> ListPlanDesarrollo { get; set; }
        
        public MItblPlanDesarrollo PlanDesarrollo { get; set; }
        
        public IEnumerable<MItblPlanDesarrolloEstructura> ListPlanDesarrolloEstructura { get; set; }
        
        public IEnumerable<GRtblControlMaestro> ListMITipoPlanDesarrollo { get; set; }
        
        public string EjercicioUsuario { get; set; }
    }
}