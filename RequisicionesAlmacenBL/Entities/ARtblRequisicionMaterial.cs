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
    
    public partial class ARtblRequisicionMaterial
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ARtblRequisicionMaterial()
        {
            this.ARtblRequisicionMaterialDetalle = new HashSet<ARtblRequisicionMaterialDetalle>();
        }
    
        public int RequisicionMaterialId { get; set; }
        public string CodigoRequisicion { get; set; }
        public System.DateTime FechaRequisicion { get; set; }
        public string AreaId { get; set; }
        public string UnidadAdministrativaId { get; set; }
        public string ProyectoId { get; set; }
        public int EstatusId { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public int CreadoPorId { get; set; }
        public Nullable<System.DateTime> FechaUltimaModificacion { get; set; }
        public Nullable<int> ModificadoPorId { get; set; }
        public byte[] Timestamp { get; set; }
        public int EmpleadoId { get; set; }
    
        public virtual GRtblControlMaestro GRtblControlMaestro { get; set; }
        public virtual tblDependencia tblDependencia { get; set; }
        public virtual tblDependencia tblDependencia1 { get; set; }
        public virtual tblProyecto tblProyecto { get; set; }
        public virtual RHtblEmpleado RHtblEmpleado { get; set; }
        public virtual GRtblUsuario GRtblUsuario { get; set; }
        public virtual GRtblUsuario GRtblUsuario1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ARtblRequisicionMaterialDetalle> ARtblRequisicionMaterialDetalle { get; set; }
    }
}
