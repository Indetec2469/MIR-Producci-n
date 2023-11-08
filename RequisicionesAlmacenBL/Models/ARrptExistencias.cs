using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(ARrptExistenciasMetaData))]
    public partial class ARrptExistencias
    {
        public static List<string> PropiedadesNoActualizables = new List<string>();

        //Filtros
        public int Id { get; set; }
        public string UnidadAdministrativaId { get; set; }
        public string ProyectoId { get; set; }
        public string FuenteFinanciamientoId { get; set; }
        public string TipoGastoId { get; set; }
        public string ObjetoGastoId { get; set; }        
    }

    public class ARrptExistenciasMetaData
    {
        [Display(Name = "Tipo de Reporte")]
        [Required(ErrorMessage = "Tipo de Reporte requerido.")]
        public int Id { get; set; }                

        [Display(Name = "Unidad Administrativa")]
        public string UnidadAdministrativaId { get; set; }

        [Display(Name = "Proyecto")]
        public string ProyectoId { get; set; }
        
        [Display(Name = "Fuente de Financiamiento")]
        public string FuenteFinanciamientoId { get; set; }

        [Display(Name = "Tipo de Gasto")]
        public string TipoGastoId { get; set; }

        [Display(Name = "Partida Presupuestaria")] 
        public string ObjetoGastoId { get; set; }        
    }
}