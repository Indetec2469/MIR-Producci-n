using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(ARtblRequisicionMaterialMetaData))]
    public partial class ARtblRequisicionMaterial
    {
        public static List<string> PropiedadesNoActualizables = new List<string>() {
            "CreadoPorId",
            "FechaCreacion"
        };

        public string NumeroOficio { get; set; }

        public string Observaciones { get; set; }

        public string Estatus { get; set; }

        public ARtblRequisicionMaterial GetModelo(ARtblRequisicionMaterial obj)
        {
            ARtblRequisicionMaterial modelo = new ARtblRequisicionMaterial();

            modelo.RequisicionMaterialId = obj.RequisicionMaterialId;
            modelo.CodigoRequisicion = obj.CodigoRequisicion;
            modelo.FechaRequisicion = obj.FechaRequisicion;
            modelo.AreaId = obj.AreaId;
            modelo.UnidadAdministrativaId = obj.UnidadAdministrativaId;
            modelo.ProyectoId = obj.ProyectoId;
            modelo.EstatusId = obj.EstatusId;
            modelo.FechaCreacion = obj.FechaCreacion;
            modelo.CreadoPorId = obj.CreadoPorId;
            modelo.FechaUltimaModificacion = obj.FechaUltimaModificacion;
            modelo.ModificadoPorId = obj.ModificadoPorId;
            modelo.Timestamp = obj.Timestamp;

            return modelo;
        }
    }

    public class ARtblRequisicionMaterialMetaData
    {
        [Display(Name = "Código Requisición")]
        [Required(ErrorMessage = "Código de Requisición requerido")]
        public string CodigoRequisicion { get; set; }

        [Display(Name = "Fecha de Requisición")]
        [Required(ErrorMessage = "Fecha de Requisición requerida")]
        public DateTime FechaRequisicion { get; set; }

        [Display(Name = "Área")]
        [Required(ErrorMessage = "Área requerida")]
        public string AreaId { get; set; }

        [Display(Name = "Unidad Administrativa")]
        public string UnidadAdministrativaId { get; set; }

        [Display(Name = "Proyecto")]
        public string ProyectoId { get; set; }

        [Display(Name = "Empleado")]
        [Required(ErrorMessage = "Empleado requerido")]
        public int EmpleadoId { get; set; }
    }
}