using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(ARtblInvitacionCompraMetaData))]
    public partial class ARtblInvitacionCompra
    {
        public static List<string> PropiedadesNoActualizables = new List<string>() {
            "Fecha",
            "CreadoPorId",
            "FechaCreacion"
        };

        public int Proveedores { get; set; }
        public string FechaInvitacion { get; set; }
        public string Estatus { get; set; }
        public string FechaDesiertaS { get; set; }

        public ARtblInvitacionCompra GetModelo(ARtblInvitacionCompra obj)
        {
            ARtblInvitacionCompra modelo = new ARtblInvitacionCompra();

            modelo.InvitacionCompraId = obj.InvitacionCompraId;
            modelo.CodigoInvitacion = obj.CodigoInvitacion;
            modelo.ProveedorId = obj.ProveedorId;
            modelo.AlmacenId = obj.AlmacenId;
            modelo.Ejercicio = obj.Ejercicio;
            modelo.Fecha = obj.Fecha;
            modelo.MontoInvitacion = obj.MontoInvitacion;
            modelo.FechaDesierta = obj.FechaDesierta;
            modelo.Observacion = obj.Observacion;
            modelo.EstatusId = obj.EstatusId;
            modelo.FechaCreacion = obj.FechaCreacion;
            modelo.CreadoPorId = obj.CreadoPorId;
            modelo.FechaUltimaModificacion = obj.FechaUltimaModificacion;
            modelo.ModificadoPorId = obj.ModificadoPorId;
            modelo.Timestamp = obj.Timestamp;

            return modelo;
        }
    }

    public class ARtblInvitacionCompraMetaData
    {
        
    }   
}
