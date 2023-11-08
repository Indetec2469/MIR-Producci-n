using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ExpressiveAnnotations.Attributes;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(ARtblProveedorProspectoMetaData))]
    public partial class ARtblProveedorProspecto
    {
        public static List<string> PropiedadesNoActualizables = new List<string>() {
            "CodigoProspecto",
            "CreadoPorId",
            "FechaCreacion"
        };
    }

    public class ARtblProveedorProspectoMetaData
    {
        [Display(Name = "Código")]
        [Required(ErrorMessage = "El Código es requerido")]
        public string CodigoProspecto { get; set; }

        [Display(Name = "RFC")]
        [Required(ErrorMessage = "El RFC es requerido")]
        public string RFC { get; set; }

        [Display(Name = "Razón Social")]
        [Required(ErrorMessage = "La Razón Social es requerida")]
        public string RazonSocial { get; set; }

        [Display(Name = "Nombre de Contacto")]
        [Required(ErrorMessage = "El Nombre de Contacto es requerido")]
        public string NombreContacto { get; set; }

        [Display(Name = "Primer Apellido")]
        [Required(ErrorMessage = "El Primer Apellido es requerido")]
        public string PrimerApellido { get; set; }

        [Display(Name = "Segundo Apellido")]
        public string SegundoApellido { get; set; }

        [Display(Name = "Teléfono")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Teléfono Inválido")]
        [Required(ErrorMessage = "El Teléfono es requerido")]
        public string Telefono { get; set; }

        [Display(Name = "Extensión")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Extensión Inválida")]
        public string Extension { get; set; }

        [Display(Name = "Correo Electrónico")]
        [RegularExpression(@"^[\d\w\._\-]+@([\d\w\._\-]+\.)+[\w]+$", ErrorMessage = "Correo Electrónico Inválido")]
        public string CorreoElectronico { get; set; }

        [Display(Name = "Comentarios")]
        public string Comentarios { get; set; }

        [Display(Name = "País")]
        [Required(ErrorMessage = "El País es requerido")] 
        public string PaisId { get; set; }

        [Display(Name = "Estado")]
        public string EstadoId { get; set; }

        [Display(Name = "Municipio")]
        public Nullable<int> MunicipioId { get; set; }

        [Display(Name = "Tipo de Proveedor")]
        [Required(ErrorMessage = "El Tipo de Proveedor es requerido")]
        public string TipoProveedorId { get; set; }

        [Display(Name = "Tipo de Operacion")]
        [Required(ErrorMessage = "El Tipo de Operacion es requerido")]
        public string TipoOperacionId { get; set; }

        [Display(Name = "Tipo de Comprobante Fiscal")]
        [Required(ErrorMessage = "El Tipo de Comprobante Fiscal es requerido")]
        public Nullable<int> TipoComprobanteFiscalId { get; set; }
    }
}