
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
    
public partial class ARtblCortesia
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ARtblCortesia()
    {

        this.ARtblCortesiaDetalle = new HashSet<ARtblCortesiaDetalle>();

    }


    public int CortesiaId { get; set; }

    public string Codigo { get; set; }

    public System.DateTime Fecha { get; set; }

    public int ProveedorId { get; set; }

    public string AlmacenId { get; set; }

    public Nullable<int> OrdenCompraId { get; set; }

    public string Comentarios { get; set; }

    public Nullable<bool> AfectaCostoPromedio { get; set; }

    public Nullable<double> TotalCortesia { get; set; }

    public int EstatusId { get; set; }

    public System.DateTime FechaCreacion { get; set; }

    public int CreadoPorId { get; set; }

    public Nullable<System.DateTime> FechaUltimaModificacion { get; set; }

    public Nullable<int> ModificadoPorId { get; set; }

    public byte[] Timestamp { get; set; }



    public virtual GRtblControlMaestro GRtblControlMaestro { get; set; }

    public virtual tblAlmacen tblAlmacen { get; set; }

    public virtual tblOrdenCompra tblOrdenCompra { get; set; }

    public virtual tblProveedor tblProveedor { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<ARtblCortesiaDetalle> ARtblCortesiaDetalle { get; set; }

    public virtual GRtblUsuario GRtblUsuario { get; set; }

    public virtual GRtblUsuario GRtblUsuario1 { get; set; }

}

}
