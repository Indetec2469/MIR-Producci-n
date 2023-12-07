
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------


namespace RequisicionesAlmacenBL.Entities
{

using System;
    
public partial class ARspConsultaRequisicionPorComprarDetalles_Result
{

    public int RequisicionMaterialDetalleId { get; set; }

    public int RequisicionMaterialId { get; set; }

    public string Solicitud { get; set; }

    public string Fecha { get; set; }

    public System.DateTime FechaRequisicion { get; set; }

    public string Usuario { get; set; }

    public string AreaId { get; set; }

    public string Area { get; set; }

    public string AlmacenId { get; set; }

    public string Almacen { get; set; }

    public string UnidadAdministrativaId { get; set; }

    public string UnidadAdministrativa { get; set; }

    public string ProyectoId { get; set; }

    public string Proyecto { get; set; }

    public string TipoGastoId { get; set; }

    public string TipoGasto { get; set; }

    public string ProductoId { get; set; }

    public string Descripcion { get; set; }

    public string UM { get; set; }

    public decimal CostoUnitario { get; set; }

    public string TarifaImpuestoId { get; set; }

    public decimal CantidadSolicitada { get; set; }

    public Nullable<decimal> CantidadSurtida { get; set; }

    public decimal Existencia { get; set; }

    public Nullable<decimal> ExistenciaMinima { get; set; }

    public Nullable<double> CantidadComprar { get; set; }

    public Nullable<double> Total { get; set; }

    public Nullable<double> Ajuste { get; set; }

    public string Comentarios { get; set; }

    public string FuenteFinanciamientoId { get; set; }

    public Nullable<int> DisponibleComprometer { get; set; }

    public Nullable<int> PreComprometido { get; set; }

    public Nullable<int> ProveedorId { get; set; }

    public int EstatusId { get; set; }

    public string Estatus { get; set; }

    public Nullable<bool> Revision { get; set; }

    public Nullable<bool> Comprar { get; set; }

    public Nullable<bool> Rechazar { get; set; }

    public string Motivo { get; set; }

    public Nullable<int> OrdenCompraDetId { get; set; }

    public Nullable<int> OrdenCompraId { get; set; }

    public Nullable<int> CuentaPresupuestalEgrId { get; set; }

    public Nullable<decimal> Importe { get; set; }

    public Nullable<double> IEPS { get; set; }

    public Nullable<double> IVA { get; set; }

    public Nullable<double> ISH { get; set; }

    public Nullable<double> RetencionISR { get; set; }

    public Nullable<double> RetencionCedular { get; set; }

    public Nullable<double> RetencionIVA { get; set; }

    public Nullable<double> TotalPresupuesto { get; set; }

    public Nullable<int> InvitacionCompraDetalleId { get; set; }

    public Nullable<int> InvitacionCompraId { get; set; }

    public byte[] RequisicionTimestamp { get; set; }

    public byte[] DetalleTimestamp { get; set; }

}

}
