using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(MItblPlanDesarrolloMetaData))]
    public partial class MItblPlanDesarrollo
    {
        public static List<string> PropiedadesNoActualizables = new List<string>();

        //public PlanNacionalDesarrollo() { }
    }

    public class MItblPlanDesarrolloMetaData
    {
        [Display(Name = "Código")]
        public string PlanDesarrolloId { get; set; }

        [Display(Name = "Nombre del Plan")]
        [Required(ErrorMessage = "Nombre del Plan requerido")]
        public string NombrePlan { get; set; }

        [Display(Name = "Fecha Inicio")]
        [Required(ErrorMessage = "Fecha Inicio requerida")]
        public DateTime FechaInicio { get; set; }

        [Display(Name = "Fecha Fin")]
        [Required(ErrorMessage = "Fecha Fin requerida")]
        public DateTime FechaFin { get; set; }

        [Display(Name = "Tipo de Plan")]
        [Required(ErrorMessage = "Tipo de Plan requerido")]
        public int TipoPlanId { get; set; }
    }
}