namespace RequisicionesAlmacenBL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    [MetadataType(typeof(ConfiguracionAreaAlmacenItemMetaData))]
    public partial class ConfiguracionAreaAlmacenItem
    {
        public int ConfiguracionAreaAlmacenId { get; set; }
        public int ConfiguracionAreaId { get; set; }
        public string AlmacenId { get; set; }
        public bool Borrado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int CreadoPorId { get; set; }
        public Nullable<DateTime> FechaUltimaModificacion { get; set; }
        public Nullable<int> ModificadoPorId { get; set; }
        public byte[] Timestamp { get; set; }
        public int EstatusId { get; set; }

        public static explicit operator ARtblControlMaestroConfiguracionAreaAlmacen(ConfiguracionAreaAlmacenItem obj)
        {
            ARtblControlMaestroConfiguracionAreaAlmacen modelo = new ARtblControlMaestroConfiguracionAreaAlmacen();

            modelo.ConfiguracionAreaAlmacenId = obj.ConfiguracionAreaAlmacenId;
            modelo.ConfiguracionAreaId = obj.ConfiguracionAreaId;
            modelo.AlmacenId = obj.AlmacenId;
            modelo.Borrado = obj.Borrado;
            modelo.FechaCreacion = obj.FechaCreacion;
            modelo.CreadoPorId = obj.CreadoPorId;
            modelo.FechaUltimaModificacion = obj.FechaUltimaModificacion;
            modelo.ModificadoPorId = obj.ModificadoPorId;
            modelo.Timestamp = obj.Timestamp;

            return modelo;
        }
    }

    public class ConfiguracionAreaAlmacenItemMetaData
    {

    }
}