using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(ARtblInvitacionArticuloMetaData))]
    public partial class ARtblInvitacionArticulo
    {
        public static List<string> PropiedadesNoActualizables = new List<string>() {
            "CreadoPorId",
            "FechaCreacion"
        };

        public ARtblInvitacionArticulo GetModelo(ARtblInvitacionArticulo obj)
        {
            ARtblInvitacionArticulo modelo = new ARtblInvitacionArticulo();

            modelo.InvitacionArticuloId = obj.InvitacionArticuloId;
            modelo.ProveedorId = obj.ProveedorId;
            modelo.AlmacenId = obj.AlmacenId;
            modelo.MontoInvitacion = obj.MontoInvitacion;
            modelo.TipoCompraId = obj.TipoCompraId;
            modelo.EstatusId = obj.EstatusId;
            modelo.FechaCreacion = obj.FechaCreacion;
            modelo.CreadoPorId = obj.CreadoPorId;
            modelo.FechaUltimaModificacion = obj.FechaUltimaModificacion;
            modelo.ModificadoPorId = obj.ModificadoPorId;
            modelo.Timestamp = obj.Timestamp;

            return modelo;
        }
    }

    public class ARtblInvitacionArticuloMetaData
    {
        
    }
}