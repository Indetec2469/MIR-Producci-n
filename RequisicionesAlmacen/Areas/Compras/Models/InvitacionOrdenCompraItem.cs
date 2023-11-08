namespace RequisicionesAlmacenBL.Entities
{
    using RequisicionesAlmacen.Helpers;
    using RequisicionesAlmacenBL.Models.Mapeos;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(InvitacionOrdenCompraItemMetaData))]
    public partial class InvitacionOrdenCompraItem
    {
        //Propiedades de la Orden de Compra
        public int OrdenCompraId { get; set; }
        public int ProveedorId { get; set; }
        public string AlmacenId { get; set; }
        public string TipoOperacionId { get; set; }
        public int TipoComprobanteFiscalId { get; set; }
        public int Ejercicio { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaRecepcion { get; set; }
        public string Referencia { get; set; }
        public string Observacion { get; set; }
        public Nullable<int> GastoPorComprobarId { get; set; }

        //Propiedades del Item
        public string Proveedor { get; set; }
        public string Almacen { get; set; }
        public decimal Monto { get; set; }
        public List<InvitacionOrdenCompraDetalleItem> Detalles { get; set; }

        public static explicit operator tblOrdenCompra(InvitacionOrdenCompraItem obj)
        {
            tblOrdenCompra modelo = new tblOrdenCompra();

            modelo.OrdenCompraId = obj.OrdenCompraId;
            modelo.ProveedorId = obj.ProveedorId;
            modelo.AlmacenId = obj.AlmacenId;
            modelo.TipoOperacionId = obj.TipoOperacionId;
            modelo.TipoComprobanteFiscalId = obj.TipoComprobanteFiscalId;
            modelo.Ejercicio = obj.Ejercicio;
            modelo.Fecha = obj.Fecha;
            modelo.FechaRecepcion = obj.FechaRecepcion;
            modelo.Referencia = obj.Referencia;
            modelo.Status = "A";
            modelo.Observacion = obj.Observacion;
            modelo.GastoPorComprobarId = obj.GastoPorComprobarId;

            foreach (InvitacionOrdenCompraDetalleItem detalleItem in obj.Detalles)
            {
                modelo.tblOrdenCompraDet.Add((tblOrdenCompraDet)detalleItem);
            }

            return modelo;
        }
    }

    public class InvitacionOrdenCompraItemMetaData
    {
        [Display(Name = "Proveedor")]
        [Required(ErrorMessage = "Proveedor requerido")]
        public string Proveedor { get; set; }

        [Display(Name = "Almacén")]
        [Required(ErrorMessage = "Almacén requerido")]
        public string Almacen { get; set; }

        [Display(Name = "Fecha Recepción")]
        [Required(ErrorMessage = "Fecha Recepción requerida")] 
        public DateTime FechaRecepcion { get; set; }

        [Display(Name = "Monto Orden de Compra")]
        [Required(ErrorMessage = "Monto Orden de Compra requerido")]
        public decimal Monto { get; set; }

        [StringLength(1000)]
        [Display(Name = "Comentarios")]
        public string Observacion { get; set; }
    }
}