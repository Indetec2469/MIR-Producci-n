using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(ARtblControlMaestroConceptoAjusteInventarioMetaData))]
    public partial class ARtblControlMaestroConceptoAjusteInventario
    {
        public static List<string> PropiedadesNoActualizables = new List<string>();
    }

    public class ARtblControlMaestroConceptoAjusteInventarioMetaData
    {
        [Display(Name = "Código")]
        public int ConceptoAjusteInventarioId { get; set; }

        [Required(ErrorMessage = "Concepto Movimiento requerido")]
        [MaxLength(150, ErrorMessage = "El máximo es 150 carácteres.")]
        [MinLength(1, ErrorMessage = "El mínimo es 1 carácter.")]
        //[Remote("IsConceptoAjusteSigned", "ConceptoAjusteInventario", AdditionalFields = "ConceptoAjusteInventarioId,TipoMovimientoId ", HttpMethod = "POST", ErrorMessage = "El concepto ajuste ya existe, elige un concepto ajuste otro.")]
        [Display(Name = "Concepto Movimiento")]
        public string ConceptoAjuste { get; set; }

        [Required(ErrorMessage = "Tipo de Movimiento requerido")]
        //[Remote("IsTipoMovimientoSigned", "ConceptoAjusteInventario", HttpMethod = "POST", ErrorMessage = "El tipo de movimiento ya existe, elige un tipo de movimiento otro.")]
        [Display(Name = "Tipo de Movimiento")]
        public int TipoMovimientoId { get; set; }

        [Required(ErrorMessage = "Solicita Archivo Evidencia requerido")]
        [Display(Name = "Solicita archivo evidencia")]
        public bool SolicitaEvidencia { get; set; }
    }
}
