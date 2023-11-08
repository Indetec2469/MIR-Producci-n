using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(ARtblInvitacionCompraDetalleMetaData))]
    public partial class ARtblInvitacionCompraDetalle
    {
        public static List<string> PropiedadesNoActualizables = new List<string>() {
            "CreadoPorId",
            "FechaCreacion"
        };

        public ARtblInvitacionCompraDetalle GetModelo(ARtblInvitacionCompraDetalle obj)
        {
            ARtblInvitacionCompraDetalle modelo = new ARtblInvitacionCompraDetalle();

            modelo.InvitacionCompraDetalleId = obj.InvitacionCompraDetalleId;
            modelo.InvitacionCompraId = obj.InvitacionCompraId;
            modelo.TarifaImpuestoId = obj.TarifaImpuestoId;
            modelo.ProductoId = obj.ProductoId;
            modelo.CuentaPresupuestalEgrId = obj.CuentaPresupuestalEgrId;
            modelo.Descripcion = obj.Descripcion;
            modelo.Cantidad = obj.Cantidad;
            modelo.Costo = obj.Costo;
            modelo.Importe = obj.Importe;
            modelo.IEPS = obj.IEPS;
            modelo.Ajuste = obj.Ajuste;
            modelo.IVA = obj.IVA;
            modelo.ISH = obj.ISH;
            modelo.RetencionISR = obj.RetencionISR;
            modelo.RetencionCedular = obj.RetencionCedular;
            modelo.RetencionIVA = obj.RetencionIVA;
            modelo.TotalPresupuesto = obj.TotalPresupuesto;
            modelo.Total = obj.Total;
            modelo.EstatusId = obj.EstatusId;
            modelo.FechaCreacion = obj.FechaCreacion;
            modelo.CreadoPorId = obj.CreadoPorId;
            modelo.FechaUltimaModificacion = obj.FechaUltimaModificacion;
            modelo.ModificadoPorId = obj.ModificadoPorId;
            modelo.Timestamp = obj.Timestamp;
            modelo.InvitacionArticuloDetalleId = obj.InvitacionArticuloDetalleId;

            return modelo;
        }
    }

    public class ARtblInvitacionCompraDetalleMetaData
    {
        
    }
}