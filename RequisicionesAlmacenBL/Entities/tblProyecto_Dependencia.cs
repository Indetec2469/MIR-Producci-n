
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
    
public partial class tblProyecto_Dependencia
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public tblProyecto_Dependencia()
    {

        this.ARtblControlMaestroConfiguracionAreaProyecto = new HashSet<ARtblControlMaestroConfiguracionAreaProyecto>();

    }


    public int idPD { get; set; }

    public string ProyectoId { get; set; }

    public string DependenciaId { get; set; }



    public virtual tblProyecto tblProyecto { get; set; }

    public virtual tblDependencia tblDependencia { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<ARtblControlMaestroConfiguracionAreaProyecto> ARtblControlMaestroConfiguracionAreaProyecto { get; set; }

}

}
