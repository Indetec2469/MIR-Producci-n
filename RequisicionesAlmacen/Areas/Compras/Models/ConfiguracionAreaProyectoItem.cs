namespace RequisicionesAlmacenBL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    [MetadataType(typeof(ConfiguracionAreaProyectoItemMetaData))]
    public partial class ConfiguracionAreaProyectoItem
    {
        public int ConfiguracionAreaProyectoId { get; set; }
        public int ConfiguracionAreaId { get; set; }
        public int ProyectoDependenciaId { get; set; }
        public bool Borrado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int CreadoPorId { get; set; }
        public Nullable<DateTime> FechaUltimaModificacion { get; set; }
        public Nullable<int> ModificadoPorId { get; set; }
        public byte[] Timestamp { get; set; }
        public int EstatusId { get; set; }

        public static explicit operator ARtblControlMaestroConfiguracionAreaProyecto(ConfiguracionAreaProyectoItem obj)
        {
            ARtblControlMaestroConfiguracionAreaProyecto modelo = new ARtblControlMaestroConfiguracionAreaProyecto();

            modelo.ConfiguracionAreaProyectoId = obj.ConfiguracionAreaProyectoId;
            modelo.ConfiguracionAreaId = obj.ConfiguracionAreaId;
            modelo.ProyectoDependenciaId = obj.ProyectoDependenciaId;
            modelo.Borrado = obj.Borrado;
            modelo.FechaCreacion = obj.FechaCreacion;
            modelo.CreadoPorId = obj.CreadoPorId;
            modelo.FechaUltimaModificacion = obj.FechaUltimaModificacion;
            modelo.ModificadoPorId = obj.ModificadoPorId;
            modelo.Timestamp = obj.Timestamp;

            return modelo;
        }
    }

    public class ConfiguracionAreaProyectoItemMetaData
    {

    }
}