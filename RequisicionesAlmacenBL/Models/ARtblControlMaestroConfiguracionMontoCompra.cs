using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(ARtblControlMaestroConfiguracionMontoCompraMetaData))]
    public partial class ARtblControlMaestroConfiguracionMontoCompra
    {
        public static List<string> PropiedadesNoActualizables = new List<string>();
        
    }

    public class ARtblControlMaestroConfiguracionMontoCompraMetaData
    {
        [Required(ErrorMessage = "Tipo de compra requerido")]
        [Display(Name = "Tipo de compra")]
        public int TipoCompraId { get; set; }

        [Required(ErrorMessage = "Monto mínimo requerido")]
        [Display(Name = "Monto mínimo")]
        [Range(0, Int32.MaxValue, ErrorMessage = "El monto debe ser positivo")]
        public decimal MontoMinimo { get; set; }

        [Display(Name = "Monto máximo")]
        public decimal MontoMaximo { get; set; }

        [Display(Name = "Cantidad mínima de proveedores")]
        [Range(1, Int32.MaxValue, ErrorMessage = "El mínimo es 1 o mas")]
        public byte NumeroMinimoProveedores { get; set; }
    }
}
