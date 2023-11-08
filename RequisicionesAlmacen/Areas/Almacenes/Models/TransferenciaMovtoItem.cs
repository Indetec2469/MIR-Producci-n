namespace RequisicionesAlmacenBL.Entities
{
    using RequisicionesAlmacen.Helpers;
    using RequisicionesAlmacenBL.Models.Mapeos;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(TransferenciaMovtoItemMetaData))]
    public partial class TransferenciaMovtoItem
    {
        //Propiedades de la Transferencia Movto
        public int TransferenciaMovtoId { get; set; }
        public int TransferenciaId { get; set; }
        public Nullable<int> NumeroLinea { get; set; }
        public string ProductoId { get; set; }
        public string Descripcion { get; set; }
        public double Cantidad { get; set; }
        public int UnidadMedidaId { get; set; }
        public int AlmacenProductoOrigenId { get; set; }
        public int AlmacenProductoDestinoId { get; set; }

        //Propiedades del Item
        public string UnidadDeMedida { get; set; }
        public int CuentaPresupuestalOrigenId { get; set; }
        public string UnidadAdministrativaOrigenId { get; set; }
        public string UnidadAdministrativaOrigen { get; set; }
        public string ProyectoOrigenId { get; set; }
        public string ProyectoOrigen { get; set; }
        public string FuenteFinanciamientoOrigenId { get; set; }
        public string FuenteFinanciamientoOrigen { get; set; }
        public string TipoGastoOrigenId { get; set; }
        public string TipoGastoOrigen { get; set; }
        public int CuentaPresupuestalDestinoId { get; set; }
        public string UnidadAdministrativaDestinoId { get; set; }
        public string UnidadAdministrativaDestino { get; set; }
        public string ProyectoDestinoId { get; set; }
        public string ProyectoDestino { get; set; }
        public string FuenteFinanciamientoDestinoId { get; set; }
        public string FuenteFinanciamientoDestino { get; set; }
        public string TipoGastoDestinoId { get; set; }
        public string TipoGastoDestino { get; set; }
        public string AlmacenDestinoId { get; set; }
        public string AlmacenDestino { get; set; }

        public static explicit operator ARtblTransferenciaMovto(TransferenciaMovtoItem obj)
        {
            ARtblTransferenciaMovto modelo = new ARtblTransferenciaMovto();

            modelo.TransferenciaMovtoId = obj.TransferenciaMovtoId;
            modelo.TransferenciaId = obj.TransferenciaId;
            modelo.NumeroLinea = obj.NumeroLinea;
            modelo.ProductoId = obj.ProductoId;
            modelo.Descripcion = obj.Descripcion;
            modelo.Cantidad = obj.Cantidad;
            modelo.UnidadMedidaId = obj.UnidadMedidaId;
            modelo.AlmacenProductoOrigenId = obj.AlmacenProductoOrigenId;
            modelo.AlmacenProductoDestinoId = obj.AlmacenProductoDestinoId;

            return modelo;
        }
    }

    public class TransferenciaMovtoItemMetaData
    {
        [Display(Name = "Producto")]
        [Required(ErrorMessage = "Producto requerido")]
        public string AlmacenProductoOrigenId { get; set; }

        [Display(Name = "Destino")]
        [Required(ErrorMessage = "Destino requerido")]
        public string AlmacenProductoDestinoId { get; set; }

        [Display(Name = "Origen")]
        public string CuentaPresupuestalOrigenId { get; set; }

        [Display(Name = "Destino")]
        public string CuentaPresupuestalDestinoId { get; set; }

        [Display(Name = "Almacén")]
        public string AlmacenDestinoId { get; set; }

        [Display(Name = "Almacén")] 
        public string AlmacenDestino { get; set; }

        [Display(Name = "UM")]
        [Required(ErrorMessage = "UM requerida")]
        public int UnidadMedidaId { get; set; }
        
        [Display(Name = "UM")]
        [Required(ErrorMessage = "UM requerida")] 
        public string UnidadDeMedida { get; set; }
        
        [Display(Name = "Producto")]
        [Required(ErrorMessage = "Producto requerido")]
        public string Descripcion { get; set; }

        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "Cantidad requerida")]
        public double Cantidad { get; set; }
    }
}