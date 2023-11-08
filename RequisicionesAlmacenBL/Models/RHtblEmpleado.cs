using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(RHtblEmpleadoMetaData))]
    public partial class RHtblEmpleado
    {
        public static List<string> PropiedadesNoActualizables = new List<string>();

    }

    public class RHtblEmpleadoMetaData
    {
        [StringLength(20)]
        [Display(Name = "Número Empleado")]
        [Required(ErrorMessage = "Número de Empleado requerido")]        
        public string NumeroEmpleado { get; set; }

        [StringLength(13)]
        [Display(Name = "RFC")]
        [Required(ErrorMessage = "RFC requerido")]
        public string RFC { get; set; }

        [StringLength(50)]
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Nombre requerido")]
        public string Nombre { get; set; }

        [StringLength(50)]
        [Display(Name = "Primer Apellido")]
        [Required(ErrorMessage = "Primer Apellido requerido")]
        public string PrimerApellido { get; set; }

        [StringLength(50)]
        [Display(Name = "Segundo Apellido")]
        public string SegundoApellido { get; set; }

        [Display(Name = "Tipo Empleado")]
        //[Required(ErrorMessage = "Tipo Empleado requerido")]        
        public int TipoEmpleadoId { get; set; }

        [Display(Name = "Area Adscripción")]
        [Required(ErrorMessage = "Area Adscripción requerida")]
        public int AreaAdscripcionId { get; set; }

        [Display(Name = "Puesto")]
        //[Required(ErrorMessage = "Puesto requerido")]
        public int PuestoId { get; set; }

        [Display(Name = "Cargo")]
        //[Required(ErrorMessage = "Cargo requerido")]
        public int CargoId { get; set; }

        [EmailAddress(ErrorMessage = "Email invalido")]
        [StringLength(100)]
        [Display(Name = "Email Institucional")]
        //[RequiredIf("EmailInstitucional == null && EmailPersonal == null", ErrorMessage = "Email Institucional es requerido")]
        public string EmailInstitucional { get; set; }

        [EmailAddress()]
        [StringLength(100)]
        [Display(Name = "Email Personal")]
        //[RequiredIf("EmailInstitucional == null && EmailPersonal == null", ErrorMessage = "Email Personal es requerido")]
        public string EmailPersonal { get; set; }

        [Display(Name = "Vigente")]
        public bool Vigente { get; set; }
    }
}
