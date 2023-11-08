using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(MItblMatrizIndicadorResultadoIndicadorMetaData))]
    public partial class MItblMatrizIndicadorResultadoIndicador
    {
        public static List<string> PropiedadesNoActualizables = new List<string>();

    }

    public class MItblMatrizIndicadorResultadoIndicadorMetaData
    {
        [Display(Name = "Nivel")]
        public int NivelIndicadorId { get; set; }
        
        [Display(Name = "Nombre del Indicador")]
        [StringLength(500, ErrorMessage = "La máxima de 500 caracteres ")]
        //[MinLength(5, ErrorMessage = "La mínima de 2 caracteres")]
        [Required(ErrorMessage = "Nombre del Indicador requerido")]
        public string NombreIndicador { get; set; }

        [Display(Name = "Resumen Narrativo")]
        [Required(ErrorMessage = "Resumen Narrativo requerido")]
        public string ResumenNarrativo { get; set; }

        [Display(Name = "Defeinición del indicador")]
        [Required(ErrorMessage = "Defeinición del indicador requerido")]
        public string DefinicionIndicador { get; set; }

        [Display(Name = "Tipo de indicador")]
        [Required(ErrorMessage = "Tipo de indicador requerido")]
        public int TipoIndicadorId { get; set; }

        [Display(Name = "Dimensión")]
        [Required(ErrorMessage = "Dimensión requerido")]
        public int DimensionId { get; set; }

        [Display(Name = "Unidad de medida")]
        [Required(ErrorMessage = "Unidad de medida requerido")]
        public int UnidadMedidaId { get; set; }

        [Display(Name = "Frecuencia de medición")]
        [Required(ErrorMessage = "Frecuencia de medición requerido")]
        public int FrecuenciaMedicionId { get; set; }

        [Display(Name = "Sentido")]
        [Required(ErrorMessage = "Sentido requerido")]
        public int SentidoId { get; set; }

        [Display(Name = "Tipo de componente")]
        public int TipoComponenteId { get; set; }

        [Display(Name = "Proyecto")]
        public int ProyectoId { get; set; }

        [Display(Name = "Porcentaje Proyecto")]
        [Range(0, 100, ErrorMessage = "El porcentaje debe ser 0 al 100%")]
        public decimal PorcentajeProyecto{ get; set; }

        [Display(Name = "Monto Proyecto")]
        public decimal MontoProyecto { get; set; }

        [Display(Name = "Porcentaje Actividad")]
        [Range(0, 100, ErrorMessage = "El porcentaje debe ser 0 al 100%")]
        public decimal PorcentajeActividad { get; set; }

        [Display(Name = "Monto Actividad")]
        public decimal MontoActividad { get; set; }

        [Display(Name = "Año base")]
        [StringLength(4, ErrorMessage = "La máxima de 4 caracteres")]
        [Required(ErrorMessage = "Año base requerido")]
        public string AnioBase { get; set; }

        [Display(Name = "Valor")]
        [Required(ErrorMessage = "Valor requerido")]
        [Range(0, Int32.MaxValue, ErrorMessage = "El monto debe ser positivo")]
        public decimal ValorBase { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "Descripción requerido")]
        public decimal DescripcionBase { get; set; }

        [Display(Name = "Desde")]
        [Required(ErrorMessage = "Desde requerido")]
        [Range(0, 100, ErrorMessage = "El porcentaje debe ser 0 al 100%")]
        public decimal AceptableDesde { get; set; }

        [Display(Name = "Hasta")]
        [Required(ErrorMessage = "Hasta requerido")]
        [Range(0, 100, ErrorMessage = "El porcentaje debe ser 0 al 100%")]
        public decimal AceptableHasta { get; set; }

        [Display(Name = "Desde")]
        [Required(ErrorMessage = "Desde requerido")]
        [Range(0, 100, ErrorMessage = "El porcentaje debe ser 0 al 100%")]
        public decimal ConRiesgoDesde { get; set; }

        [Display(Name = "Hasta")]
        [Required(ErrorMessage = "Hasta requerido")]
        [Range(0, 100, ErrorMessage = "El porcentaje debe ser 0 al 100%")]
        public decimal ConRiesgoHasta { get; set; }

        [Display(Name = "Por debajo")]
        [Required(ErrorMessage = "Por debajo requerido")]
        [Range(0, 100, ErrorMessage = "El porcentaje debe ser 0 al 100%")]
        public decimal CriticoPorDebajo{ get; set; }

        [Display(Name = "Por encima")]
        [Required(ErrorMessage = "Por encima requerido")]
        [Range(0, 100, ErrorMessage = "El porcentaje debe ser 0 al 100%")]
        public decimal CriticoPorEncima { get; set; }

        [Display(Name = "Fórmula")]
        [Required(ErrorMessage = "Fórmula requerido")]
        public int FormulaId { get; set; }

        [Display(Name = "Descripción de la fórmula")]
        [Required(ErrorMessage = "Descripción de la fórmula requerido")]
        public string DescripcionFormula { get; set; }

        //[Display(Name = "Descripción variable 1")]
        //public string DescripcionVariable1 { get; set; }

        //[Display(Name = "Descripción variable 2")]
        //public string DescripcionVariable2 { get; set; }

        //[Display(Name = "Descripción variable 3")]
        //public string DescripcionVariable3 { get; set; }

        //[Display(Name = "Descripción variable 4")]
        //public string DescripcionVariable4 { get; set; }

        [Display(Name = "Fuentes de información")]
        [Required(ErrorMessage = "Fuentes de información requerido")]
        public string FuenteInformacion { get; set; }

        [Display(Name = "Medios de verificación")]
        [Required(ErrorMessage = "Medios de verificación requerido")]
        public string MedioVerificacion { get; set; }
    }
}
