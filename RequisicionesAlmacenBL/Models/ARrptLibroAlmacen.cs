using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(ARrptLibroAlmacenMetaData))]
    public partial class ARrptLibroAlmacen
    {
        public static List<string> PropiedadesNoActualizables = new List<string>();

        //Filtros
        public int Id { get; set; }

        public string AlmacenId { get; set; }

        public DateTime Fecha { get; set; }

        public string CuentaAlmacenCodigo { get; set; }
    }

    public class ARrptLibroAlmacenMetaData
    {
        [Display(Name = "Tipo de Reporte")]
        [Required(ErrorMessage = "Tipo de Reporte requerido.")]
        public int Id { get; set; }

        [Display(Name = "Almacén")]
        [Required(ErrorMessage = "Almacen requerido.")]
        public string AlmacenId { get; set; }

        [Display(Name = "Fecha")]
        [Required(ErrorMessage = "Fecha requerida.")]
        public DateTime Fecha { get; set; }

        [Display(Name = "Cuenta de Almacén")]
        [Required(ErrorMessage = "Cuenta de Almacén requerida.")]
        public string CuentaAlmacenCodigo { get; set; }
    }
}