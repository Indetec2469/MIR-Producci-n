using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(UsuarioSesionMetaData))]
    public class UsuarioSesion
    {

        public int UsuarioId { get; set; } 
        public string NombreUsuario { get; set; } 
        public string Contrasenia { get; set;}
        public string EnteId { get; set; }
        public string Ejercicio { get; set; }

    }

    public class UsuarioSesionMetaData
    {
        [Display(Name = "Ente")]
        [Required(ErrorMessage = "Ente Requerido")]
        public string EnteId { get; set; }

        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "Nombre de Usuario Requerido")]
        public string NombreUsuario { get; set; }

        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "Contraseña Requerida")]
        public int Contrasenia { get; set; }

    }
}