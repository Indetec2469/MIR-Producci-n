namespace RequisicionesAlmacenBL.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using LinqToExcel;
    
    [MetadataType(typeof(ImportarAlmacenProductoItemMetaData))]
    public partial class ImportarAlmacenProductoItem
    {
        public Decimal Cantidad { get; set; }
        public string ProductoId { get; set; }
        public string UnidadDeMedida { get; set; }
        public Decimal CostoUnitario { get; set; }
        public string AlmacenId { get; set; }
        public string FuenteFinanciamientoId { get; set; }
        public string ProyectoId { get; set; }
        public string UnidadAdministrativaId { get; set; }
        public string TipoGastoId { get; set; }
        public string Comentarios { get; set; }

        public string Errores { get; set; }

        public ImportarAlmacenProductoItem() { }

        public ImportarAlmacenProductoItem(Row fila)
        {
            Cantidad = Convert.ToDecimal(fila[0].ToString());
            ProductoId = fila[1].ToString();
            UnidadDeMedida = fila[2].ToString();
            CostoUnitario = Convert.ToDecimal(fila[3].ToString());
            AlmacenId = fila[4].ToString();
            FuenteFinanciamientoId = fila[5].ToString();
            ProyectoId = fila[6].ToString();
            UnidadAdministrativaId = fila[7].ToString();
            TipoGastoId = fila[8].ToString();
            Comentarios = fila[9].ToString();
        }

        public static explicit operator ARudtImportarAlmacenProducto(ImportarAlmacenProductoItem obj)
        {
            ARudtImportarAlmacenProducto modelo = new ARudtImportarAlmacenProducto();

            modelo.Cantidad = obj.Cantidad;
            modelo.ProductoId = obj.ProductoId;
            modelo.CostoUnitario = obj.CostoUnitario;
            modelo.AlmacenId = obj.AlmacenId;
            modelo.FuenteFinanciamientoId = obj.FuenteFinanciamientoId;
            modelo.ProyectoId = obj.ProyectoId;
            modelo.UnidadAdministrativaId = obj.UnidadAdministrativaId;
            modelo.TipoGastoId = obj.TipoGastoId;

            return modelo;
        }
    }

    public class ImportarAlmacenProductoItemMetaData
    {
        [Display(Name = "Código Producto")] 
        public string ProductoId { get; set; }

        [Display(Name = "UM")] 
        public string UnidadDeMedida { get; set; }

        [Display(Name = "Almacén")] 
        public string AlmacenId { get; set; }

        [Display(Name = "Fuente de Financiamiento")] 
        public string FuenteFinanciamientoId { get; set; }

        [Display(Name = "Proyecto")] 
        public string ProyectoId { get; set; }

        [Display(Name = "Unidad Administrativa")] 
        public string UnidadAdministrativaId { get; set; }

        [Display(Name = "Tipo de Gasto")] 
        public string TipoGastoId { get; set; }

        [Display(Name = "Problemas Encontrados")]
        public string Errores { get; set; }
    }
}