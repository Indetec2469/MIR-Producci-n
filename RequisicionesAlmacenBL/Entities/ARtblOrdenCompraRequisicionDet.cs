
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
    
public partial class ARtblOrdenCompraRequisicionDet
{

    public int OrdenCompraRequisicionDetId { get; set; }

    public int OrdenCompraDetId { get; set; }

    public int RequisicionMaterialDetalleId { get; set; }

    public decimal Cantidad { get; set; }

    public System.DateTime FechaCreacion { get; set; }

    public int CreadoPorId { get; set; }

    public byte[] Timestamp { get; set; }

}

}
