
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
    
public partial class GRtblMenuPrincipal
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public GRtblMenuPrincipal()
    {

        this.GRtblRolMenu = new HashSet<GRtblRolMenu>();

        this.GRtblPermisoFicha = new HashSet<GRtblPermisoFicha>();

    }


    public int NodoMenuId { get; set; }

    public string Etiqueta { get; set; }

    public string Descripcion { get; set; }

    public int TipoNodoId { get; set; }

    public Nullable<int> NodoPadreId { get; set; }

    public int SistemaAccesoId { get; set; }

    public string Url { get; set; }

    public string Icono { get; set; }

    public bool AdmitePermiso { get; set; }

    public byte Orden { get; set; }

    public int EstatusId { get; set; }

    public byte[] Timestamp { get; set; }



    public virtual GRtblControlMaestro GRtblControlMaestro { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<GRtblRolMenu> GRtblRolMenu { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<GRtblPermisoFicha> GRtblPermisoFicha { get; set; }

}

}
