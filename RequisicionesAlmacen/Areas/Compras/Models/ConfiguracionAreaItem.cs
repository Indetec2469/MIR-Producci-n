namespace RequisicionesAlmacenBL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    [MetadataType(typeof(ConfiguracionAreaItemMetaData))]
    public partial class ConfiguracionAreaItem
    {
        public int ConfiguracionAreaId { get; set; }
        public string AreaId { get; set; }
        public string Comentarios { get; set; }
        public bool Borrado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int CreadoPorId { get; set; }
        public Nullable<DateTime> FechaUltimaModificacion { get; set; }
        public Nullable<int> ModificadoPorId { get; set; }
        public byte[] Timestamp { get; set; }
        public string Codigo { get; set; }
        public string Area { get; set; }
        public int Proyectos { get; set; }
        public int UnidadesAdministrativas { get; set; }
        public int EstatusId { get; set; }
        public List<ConfiguracionAreaProyectoItem> Detalles { get; set; }
        public List<ConfiguracionAreaAlmacenItem> ListAlmacenes { get; set; }

        public static explicit operator ARtblControlMaestroConfiguracionArea(ConfiguracionAreaItem obj)
        {
            ARtblControlMaestroConfiguracionArea modelo = new ARtblControlMaestroConfiguracionArea();

            modelo.ConfiguracionAreaId = obj.ConfiguracionAreaId;
            modelo.AreaId = obj.AreaId;
            modelo.Comentarios = obj.Comentarios;
            modelo.Borrado = obj.Borrado;
            modelo.FechaCreacion = obj.FechaCreacion;
            modelo.CreadoPorId = obj.CreadoPorId;
            modelo.FechaUltimaModificacion = obj.FechaUltimaModificacion;
            modelo.ModificadoPorId = obj.ModificadoPorId;
            modelo.Timestamp = obj.Timestamp;

            modelo.ARtblControlMaestroConfiguracionAreaProyecto = new List<ARtblControlMaestroConfiguracionAreaProyecto>();

            if (obj.Detalles != null)
            {
                obj.Detalles.ForEach(registro =>
                {
                    modelo.ARtblControlMaestroConfiguracionAreaProyecto.Add((ARtblControlMaestroConfiguracionAreaProyecto)registro);
                });
            }

            modelo.ARtblControlMaestroConfiguracionAreaAlmacen = new List<ARtblControlMaestroConfiguracionAreaAlmacen>();

            if (obj.ListAlmacenes != null)
            {
                obj.ListAlmacenes.ForEach(registro =>
                {
                    modelo.ARtblControlMaestroConfiguracionAreaAlmacen.Add((ARtblControlMaestroConfiguracionAreaAlmacen)registro);
                });
            }

            return modelo;
        }
    }

    public class ConfiguracionAreaItemMetaData
    {

    }
}