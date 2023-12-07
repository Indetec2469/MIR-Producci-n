
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
    using System.Collections.Generic;
    
public partial class ARtblCortesiaDetalle
{

    public int CortesiaDetalleId { get; set; }

    public int CortesiaId { get; set; }

    public int NumeroPartida { get; set; }

    public string ProductoId { get; set; }

    public string Descripcion { get; set; }

    public int CuentaPresupuestalEgrId { get; set; }

    public int UnidadMedidaId { get; set; }

    public double Cantidad { get; set; }

    public double PrecioUnitario { get; set; }

    public double TotalPartida { get; set; }

    public int EstatusId { get; set; }

    public System.DateTime FechaCreacion { get; set; }

    public int CreadoPorId { get; set; }

    public Nullable<System.DateTime> FechaUltimaModificacion { get; set; }

    public Nullable<int> ModificadoPorId { get; set; }

    public byte[] Timestamp { get; set; }



    public virtual ARtblCortesia ARtblCortesia { get; set; }

    public virtual GRtblControlMaestro GRtblControlMaestro { get; set; }

    public virtual tblCuentaPresupuestalEgr tblCuentaPresupuestalEgr { get; set; }

    public virtual tblProducto tblProducto { get; set; }

    public virtual tblUnidadDeMedida tblUnidadDeMedida { get; set; }

    public virtual GRtblUsuario GRtblUsuario { get; set; }

    public virtual GRtblUsuario GRtblUsuario1 { get; set; }

}

}
