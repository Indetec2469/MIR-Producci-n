using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(GRtblUsuarioMetaData))]
    public partial class GRtblUsuario
    {

        public static List<string> PropiedadesNoActualizables = new List<string>();

        //public Usuario()
        //{
        //    PropiedadesNoActualizables = new List<string>() { nameof(Contrasenia), nameof(NombreUsuario) };

        //}


    }

    public class GRtblUsuarioMetaData
    {
        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "Nombre de Usuario requerido")]
        [StringLength(50, ErrorMessage = "La máxima de 50 caracteres ")]
        public string NombreUsuario { get; set; }

        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "Contraseña requerida")]
        [StringLength(500, ErrorMessage = "La máxima de 500 caracteres ")]
        public string Contrasenia { get; set; }

        [Display(Name = "Empleado")]
        [Required(ErrorMessage = "Empleado requerido")]
        public Nullable<int> EmpleadoId { get; set; }

        [Display(Name = "Rol")]
        [Required(ErrorMessage = "Rol requerida")]
        public Nullable<byte> RolId { get; set; }
    }
}
