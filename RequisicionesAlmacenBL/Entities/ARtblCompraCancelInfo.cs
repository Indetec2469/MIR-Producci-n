
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
    
public partial class ARtblCompraCancelInfo
{

    public int CompraCancelInfoId { get; set; }

    public int CompraId { get; set; }

    public System.DateTime FechaCancelacion { get; set; }

    public string Motivo { get; set; }



    public virtual tblCompra tblCompra { get; set; }

}

}
