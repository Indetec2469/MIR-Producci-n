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
    
    public partial class ARtblRequisicionMaterialDetalle
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ARtblRequisicionMaterialDetalle()
        {
            this.ARtblInvitacionArticuloDetalle = new HashSet<ARtblInvitacionArticuloDetalle>();
        }
    
        public int RequisicionMaterialDetalleId { get; set; }
        public int RequisicionMaterialId { get; set; }
        public string UnidadAdministrativaId { get; set; }
        public string ProyectoId { get; set; }
        public string TipoGastoId { get; set; }
        public string ProductoId { get; set; }
        public string Descripcion { get; set; }
        public int UnidadMedidaId { get; set; }
        public decimal CostoUnitario { get; set; }
        public decimal Cantidad { get; set; }
        public decimal TotalPartida { get; set; }
        public string Comentarios { get; set; }
        public int EstatusId { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public int CreadoPorId { get; set; }
        public Nullable<System.DateTime> FechaUltimaModificacion { get; set; }
        public Nullable<int> ModificadoPorId { get; set; }
        public byte[] Timestamp { get; set; }
        public string MotivoRechazo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ARtblInvitacionArticuloDetalle> ARtblInvitacionArticuloDetalle { get; set; }
        public virtual ARtblRequisicionMaterial ARtblRequisicionMaterial { get; set; }
        public virtual GRtblControlMaestro GRtblControlMaestro { get; set; }
        public virtual GRtblUsuario GRtblUsuario { get; set; }
        public virtual GRtblUsuario GRtblUsuario1 { get; set; }
        public virtual GRtblUsuario GRtblUsuario2 { get; set; }
        public virtual GRtblUsuario GRtblUsuario3 { get; set; }
        public virtual tblDependencia tblDependencia { get; set; }
        public virtual tblProducto tblProducto { get; set; }
        public virtual tblProyecto tblProyecto { get; set; }
        public virtual tblTipoGasto tblTipoGasto { get; set; }
        public virtual tblUnidadDeMedida tblUnidadDeMedida { get; set; }
    }
}
