using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(ARtblRequisicionMaterialDetalleMetaData))]
    public partial class ARtblRequisicionMaterialDetalle
    {
        public static List<string> PropiedadesNoActualizables = new List<string>() {
            "CreadoPorId",
            "FechaCreacion"
        };

        public ARtblRequisicionMaterialDetalle GetModelo(ARtblRequisicionMaterialDetalle obj)
        {
            ARtblRequisicionMaterialDetalle modelo = new ARtblRequisicionMaterialDetalle();

            modelo.RequisicionMaterialDetalleId = obj.RequisicionMaterialDetalleId;
            modelo.RequisicionMaterialId = obj.RequisicionMaterialId;
            modelo.UnidadAdministrativaId = obj.UnidadAdministrativaId;
            modelo.ProyectoId = obj.ProyectoId;
            modelo.TipoGastoId = obj.TipoGastoId;
            modelo.ProductoId = obj.ProductoId;
            modelo.Descripcion = obj.Descripcion;
            modelo.UnidadMedidaId = obj.UnidadMedidaId;
            modelo.CostoUnitario = obj.CostoUnitario;
            modelo.Cantidad = obj.Cantidad;
            modelo.TotalPartida = obj.TotalPartida;
            modelo.Comentarios = obj.Comentarios;
            modelo.EstatusId = obj.EstatusId;
            modelo.FechaCreacion = obj.FechaCreacion;
            modelo.CreadoPorId = obj.CreadoPorId;
            modelo.FechaUltimaModificacion = obj.FechaUltimaModificacion;
            modelo.ModificadoPorId = obj.ModificadoPorId;
            modelo.Timestamp = obj.Timestamp;

            return modelo;
        }
    }

    public class ARtblRequisicionMaterialDetalleMetaData
    {
        [Display(Name = "Unidad Administrativa")]
        [Required(ErrorMessage = "Unidad Administrativa requerida")]
        public string UnidadAdministrativaId { get; set; }

        [Display(Name = "Proyecto")]
        [Required(ErrorMessage = "Proyecto requerido")] 
        public string ProyectoId { get; set; }

        [Display(Name = "Tipo de Gasto")]
        [Required(ErrorMessage = "Tipo de Gasto requerido")] 
        public string TipoGastoId { get; set; }

        [Display(Name = "Artículo")]
        [Required(ErrorMessage = "Artículo requerido")] 
        public string ProductoId { get; set; }

        [Display(Name = "Artículo")]
        public string Descripcion { get; set; }

        [Display(Name = "UM")]
        [Required(ErrorMessage = "UM requerida")] 
        public int UnidadMedidaId { get; set; }

        [Display(Name = "Costo Unitario")]
        [Required(ErrorMessage = "Costo Unitario requerido")] 
        public decimal CostoUnitario { get; set; }

        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "Cantidad requerida")] 
        public decimal Cantidad { get; set; }

        [Display(Name = "Total")]
        [Required(ErrorMessage = "Total requerido")] 
        public decimal TotalPartida { get; set; }

        [StringLength(1000)]
        [Display(Name = "Comentarios")]
        public string Comentarios { get; set; }
    }
}