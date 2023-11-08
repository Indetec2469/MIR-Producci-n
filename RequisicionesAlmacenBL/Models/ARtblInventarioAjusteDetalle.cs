using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(ARtblInventarioAjusteDetalleMetaData))]
    public partial class ARtblInventarioAjusteDetalle
    {
        public static List<string> PropiedadesNoActualizables = new List<string>();

        public string NombreArchivoTmp { get; set; }
    }

    public class ARtblInventarioAjusteDetalleMetaData
    {
        [Display(Name = "Almacén")]
        [Required(ErrorMessage = "Almacén requerido")]
        public string AlmacenId { get; set; }

        [Display(Name = "Fuente de Financiamiento")]
        [Required(ErrorMessage = "Fuente de Financiamiento requerida")]
        public string FuenteFinanciamientoId { get; set; }

        [Display(Name = "Proyecto")]
        [Required(ErrorMessage = "ProyectoId requerido")]
        public string ProyectoId { get; set; }

        [Display(Name = "UnidadAdministrativaId")]
        [Required(ErrorMessage = "Unidad Administrativa requerida")]
        public string UnidadAdministrativaId { get; set; }

        [Display(Name = "Tipo de Gasto")]
        [Required(ErrorMessage = "Tipo de Gasto requerido")]
        public string TipoGastoId { get; set; }

        [Display(Name = "Tipo de Movimiento")]
        [Required(ErrorMessage = "Tipo de Movimiento requerido")]
        public int TipoMovimientoId { get; set; }

        [Display(Name = "Artículo")]
        [Required(ErrorMessage = "Artículo requerido")]
        public string ProductoId { get; set; }

        [Display(Name = "Concepto de Ajuste")]
        [Required(ErrorMessage = "Concepto de Ajuste requerido")]
        public int ConceptoAjusteId { get; set; }

        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "Cantidad requerida")]
        public float Cantidad { get; set; }

        [Display(Name = "Documento")]
        public string ArchivoId { get; set; }

        [Display(Name = "Comentarios")]
        public string Comentarios { get; set; }        
    }
}
