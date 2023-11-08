using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(ARtblCortesiaMetaData))]
    public partial class ARtblCortesia
    {
        public static List<string> PropiedadesNoActualizables = new List<string>() {
            "Fecha",
            "CreadoPorId",
            "FechaCreacion"
        };

        public string FechaCortesia { get; set; }
    }

    public class ARtblCortesiaMetaData
    {
        [Display(Name = "Código")]
        public string Codigo { get; set; }

        [Display(Name = "Fecha")]
        [Required(ErrorMessage = "Fecha requerida")]
        public DateTime Fecha { get; set; }

        [Display(Name = "Fecha")]
        public string FechaCortesia { get; set; }

        [Display(Name = "Orden de Compra")]
        public Nullable<int> OrdenCompraId { get; set; }

        [Display(Name = "Proveedor")]
        [Required(ErrorMessage = "Proveedor requerido")]
        public int ProveedorId { get; set; }

        [Display(Name = "Almacén")]
        [Required(ErrorMessage = "Almacén requerido")]
        public string AlmacenId { get; set; }
    }   
}