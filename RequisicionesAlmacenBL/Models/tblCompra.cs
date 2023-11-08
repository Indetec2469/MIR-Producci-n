using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(tblCompraMetaData))]
    public partial class tblCompra
    {
        public static List<string> PropiedadesNoActualizables = new List<string>();
        
        public Nullable<int> OrdenCompraId { get; set; }
        public string FechaOC { get; set; }
        public string EstatusOC { get; set; }
        public string StatusOC { get; set; }
        public string FechaRecibo { get; set; }
        public string MotivoCancelacion { get; set; }
        public DateTime FechaCancelacion { get; set; }
        public string Estatus { get; set; }
        public string PolizaOC { get; set; }
    }

    public class tblCompraMetaData
    {
        [Display(Name = "Orden de Compra")]
        [Required(ErrorMessage = "Orden de Compra requerida")]
        public int OrdenCompraId { get; set; }

        [Display(Name = "Código Recibo")]
        public int CompraId { get; set; }

        [Display(Name = "Proveedor")]
        [Required(ErrorMessage = "Proveedor requerido")]
        public int ProveedorId { get; set; }

        [Display(Name = "Almacén")]
        [Required(ErrorMessage = "Almacén requerido")]
        public string AlmacenId { get; set; }

        [Display(Name = "Tipo de Operación")]
        [Required(ErrorMessage = "Tipo de Operación requerido")]
        public string TipoOperacionId { get; set; }

        [Display(Name = "Tipo de Comprobante Fiscal")]
        [Required(ErrorMessage = "Tipo de Comprobante Fiscal requerido")]
        public int TipoComprobanteFiscalId { get; set; }

        [Display(Name = "Factura")]        
        [Required(ErrorMessage = "Factura requerida")]
        [MaxLength(25)]
        public string FolioFactura { get; set; }

        [Display(Name = "Fecha")]
        [Required(ErrorMessage = "Fecha de Orden de Compra requerida")] 
        public DateTime Fecha { get; set; }

        [Display(Name = "Fecha Vencimiento")]
        [Required(ErrorMessage = "Fecha de Vencimiento requerida")]
        public DateTime FechaVencimiento { get; set; }

        [Display(Name = "Fecha Contrarecibo")]
        [Required(ErrorMessage = "Fecha de Contrarecibo requerida")]
        public DateTime FechaContrarecibo { get; set; }

        [Display(Name = "Fecha Pago Programado")]
        [Required(ErrorMessage = "Fecha de Pago Programado requerida")] 
        public DateTime FechaPagoProgramado { get; set; }

        [Display(Name = "Observaciones")]
        [MaxLength(1000)]
        public string Observaciones { get; set; }

        [Display(Name = "Motivo de Cancelación")]
        [Required(ErrorMessage = "Motivo de Cancelación requerido")] 
        public string MotivoCancelacion { get; set; }

        [Display(Name = "Fecha de Cancelación")]
        [Required(ErrorMessage = "Fecha de Cancelación requerida")]
        public DateTime FechaCancelacion { get; set; }

        [Display(Name = "Fecha")] 
        public string FechaOC { get; set; }

        [Display(Name = "Estatus")] 
        public string EstatusOC { get; set; }

        [Display(Name = "Póliza")]
        public string PolizaOC { get; set; }
    }
}