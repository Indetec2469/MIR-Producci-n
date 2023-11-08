using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(ARtblInvitacionArticuloDetalleMetaData))]
    public partial class ARtblInvitacionArticuloDetalle
    {
        public static List<string> PropiedadesNoActualizables = new List<string>() {
            "InvitacionArticuloId",
            "CreadoPorId",
            "FechaCreacion"
        };

        public ARtblInvitacionArticuloDetalle GetModelo(ARtblInvitacionArticuloDetalle obj)
        {
            ARtblInvitacionArticuloDetalle modelo = new ARtblInvitacionArticuloDetalle();

            modelo.InvitacionArticuloDetalleId = obj.InvitacionArticuloDetalleId;
            modelo.InvitacionArticuloId = obj.InvitacionArticuloId;
            modelo.RequisicionMaterialDetalleId = obj.RequisicionMaterialDetalleId;
            modelo.ProductoId = obj.ProductoId;
            modelo.CuentaPresupuestalEgrId = obj.CuentaPresupuestalEgrId;
            modelo.TarifaImpuestoId = obj.TarifaImpuestoId;
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

            return modelo;
        }
    }

    public class ARtblInvitacionArticuloDetalleMetaData
    {
        
    }
}