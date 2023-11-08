using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(tblOrdenCompraMetaData))]
    public partial class tblOrdenCompra
    {
        public static List<string> PropiedadesNoActualizables = new List<string>();

        public string Estatus { get; set; }
        public string UnidadAdministrativaId { get; set; }
        public string ProyectoId { get; set; }
        public string FuenteFinanciamientoId { get; set; }
        public bool Ajuste { get; set; }
        public Nullable<DateTime> FechaCancelacion { get; set; }

        public tblOrdenCompra GetModelo(tblOrdenCompra obj)
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
            modelo.Status = obj.Status;
            modelo.Observacion = obj.Observacion;
            modelo.GastoPorComprobarId = obj.GastoPorComprobarId;

            return modelo;
        }
    }

    public class tblOrdenCompraMetaData
    {
        [Display(Name = "Clave Compra")]
        public int OrdenCompraId { get; set; }

        [Display(Name = "Proveedor")]
        [Required(ErrorMessage = "Proveedor requerido")]
        public int ProveedorId { get; set; }

        [Display(Name = "Almacén")]
        [Required(ErrorMessage = "Almacén requerido")]
        public string AlmacenId { get; set; }

        [Display(Name = "Tipo de Operación")]
        [Required(ErrorMessage = "Tipo de Operación requerido")]
        public string TipoOperacionId { get; set; }

        [Display(Name = "Tipo de Comprobante Fiscal")]
        [Required(ErrorMessage = "Tipo de Comprobante Fiscal requerido")]
        public int TipoComprobanteFiscalId { get; set; }
        
        [Display(Name = "Fecha")]
        [Required(ErrorMessage = "Fecha de Orden de Compra requerida")] 
        public DateTime Fecha { get; set; }

        [Display(Name = "Fecha Pago Programado")]
        [Required(ErrorMessage = "Fecha Pago Programado requerida")] 
        public DateTime FechaRecepcion { get; set; }

        [Display(Name = "Referencia")]
        [MaxLength(25)]
        public string Referencia { get; set; }

        [Display(Name = "Observaciones")]
        [MaxLength(1000)]
        public string Observacion { get; set; }

        [Display(Name = "Unidad Administrativa")] 
        public string UnidadAdministrativaId { get; set; }

        [Display(Name = "Proyecto / Proceso")]
        public string ProyectoId { get; set; }

        [Display(Name = "Fuente de Financiamiento")]
        public string FuenteFinanciamientoId { get; set; }

        [Display(Name = "Con Ajuste")] 
        public bool Ajuste { get; set; }
    }
}