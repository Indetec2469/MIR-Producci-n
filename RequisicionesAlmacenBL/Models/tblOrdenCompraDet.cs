using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(tblOrdenCompraDetMetaData))]
    public partial class tblOrdenCompraDet
    {
        public static List<string> PropiedadesNoActualizables = new List<string>();

        public int RequisicionMaterialDetalleId { get; set; }

        public int InvitacionCompraDetalleId { get; set; }

        public tblOrdenCompraDet GetModelo(tblOrdenCompraDet obj)
        {
            tblOrdenCompraDet modelo = new tblOrdenCompraDet();

            modelo.OrdenCompraDetId = obj.OrdenCompraDetId;
            modelo.OrdenCompraId = obj.OrdenCompraId;
            modelo.TarifaImpuestoId = obj.TarifaImpuestoId;
            modelo.ProductoId = obj.ProductoId;
            modelo.CuentaPresupuestalEgrId = obj.CuentaPresupuestalEgrId;
            modelo.Descripcion = obj.Descripcion;
            modelo.Status = obj.Status;
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

            return modelo;
        }
    }

    public class tblOrdenCompraDetMetaData
    {
        
    }
}