using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(GRtblAlertaConfiguracionMetaData))]
    public partial class GRtblAlertaConfiguracion
    {
        public static List<string> PropiedadesNoActualizables = new List<string>() {
            "CreadoPorId",
            "FechaCreacion"
        };
    }

    public class GRtblAlertaConfiguracionMetaData
    {
        [Display(Name = "Empleado")]
        public Nullable<int> EmpleadoId { get; set; }

        [Display(Name = "Figura")]
        public Nullable<int> FiguraId { get; set; }

        [Display(Name = "Tipo de Notificación")]
        [Required(ErrorMessage = "Tipo de Notificación requerida")]
        public int TipoNotificacionId { get; set; }
    }
}