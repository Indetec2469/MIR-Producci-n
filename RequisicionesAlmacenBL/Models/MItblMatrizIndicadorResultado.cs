using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(MItblMatrizIndicadorResultadoMetaData))]
    public partial class MItblMatrizIndicadorResultado
    {
        public static List<string> PropiedadesNoActualizables = new List<string>();

    }

    public class MItblMatrizIndicadorResultadoMetaData
    {
        [Display(Name = "Código")]
        public string Codigo { get; set; }

        [StringLength(4)]
        [Display(Name = "Ejercicio")]
        [Required(ErrorMessage = "Ejercicio requerido")]
        public string Ejercicio { get; set; }

        [Display(Name = "Plan de Desarrollo")]
        [Required(ErrorMessage = "Plan de Desarrollo requerido")]
        public int PlanDesarrolloId { get; set; }

        [Display(Name = "Población Objetivo")]
        public string PoblacionObjetivo { get; set; }

        [StringLength(6)]
        [Display(Name = "Programa Presupuestario")]
        [Required(ErrorMessage = "Programa Presupuestario requerido")]
        public string ProgramaPresupuestarioId { get; set; }

        [Display(Name = "Fecha Fin Configuración")]
        [Required(ErrorMessage = "Fecha Fin Configuración requerida")]
        public DateTime FechaFinConfiguracion { get; set; }

        [Display(Name = "Último Nivel del Plan de Desarrollo relacionado a la MIR")]
        [Required(ErrorMessage = "Último Nivel del Plan de Desarrollo relacionado a la MIR Requerido")]
        public int PlanDesarrolloEstructuraId { get; set; }
    }
}
