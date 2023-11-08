using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(ARtblTransferenciaMetaData))]
    public partial class ARtblTransferencia
    {
        public static List<string> PropiedadesNoActualizables = new List<string>() {
            "Fecha",
            "CreadoPorId",
            "FechaCreacion"
        };

        public string FechaTransferencia { get; set; }
    }

    public class ARtblTransferenciaMetaData
    {
        [Display(Name = "Código")]
        public string Codigo { get; set; }

        [Display(Name = "Fecha")]
        [Required(ErrorMessage = "Fecha requerida")]
        public DateTime Fecha { get; set; }

        [Display(Name = "Fecha")]
        public string FechaTransferencia { get; set; }

        [Display(Name = "Almacén origen")]
        [Required(ErrorMessage = "Almacén origen requerido")]
        public string AlmacenOrigenId { get; set; }
    }   
}