using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequisicionesAlmacenBL.Entities
{
    [MetadataType(typeof(ARrptKardexMetaData))]
    public partial class ARrptKardex
    {
        public static List<string> PropiedadesNoActualizables = new List<string>();               

        //Columnas Reporte
        public string Usuario { get; set; }
        public string Reporte { get; set; }
        public string FechaImpresion { get; set; }
        public string HoraImpresion { get; set; }
        public string AlmacenId { get; set; }
        public string Almacen { get; set; }
        public string Fecha { get; set; }
        public string TipoMovimiento { get; set; }
        public Nullable<int> ReferenciaMovtoId { get; set; }
        public string MotivoMovto { get; set; }
        public string Poliza { get; set; }
        public string ProductoId { get; set; }
        public string Producto { get; set; }
        public string UA { get; set; }
        public string Proyecto { get; set; }
        public string FF { get; set; }
        public string UM { get; set; }
        public Nullable<decimal> Entrada { get; set; }
        public Nullable<decimal> Salida { get; set; }
        public Nullable<decimal> Existencia { get; set; }
        public Nullable<decimal> CostoUnitario { get; set; }
        public Nullable<decimal> Total { get; set; }
        public Nullable<decimal> CostoPromedio { get; set; }

        //Filtros
        public Nullable<DateTime> FechaInicio { get; set; }
        public Nullable<DateTime> FechaFin { get; set; }
        public Nullable<int> TipoMvtoId { get; set; }
        public Nullable<int> MvtoId { get; set; }
        public Nullable<int> PolizaId { get; set; }
        public string UnidadAdministrativaId { get; set; }
        public string ProyectoId { get; set; }
        public string FuenteFinanciamientoId { get; set; }
    }

    public class ARrptKardexMetaData
    {
        [Display(Name = "Fecha Inicio")]
        public Nullable<DateTime> FechaInicio { get; set; }

        [Display(Name = "Fecha Fin")]
        public Nullable<DateTime> FechaFin { get; set; }

        [Display(Name = "Tipo de Movimiento")]
        public Nullable<int> TipoMvtoId { get; set; }

        [Display(Name = "Movimiento")]
        public Nullable<int> MvtoId { get; set; }

        [Display(Name = "Póliza")]
        public Nullable<int> PolizaId { get; set; }

        [Display(Name = "Almacén")]
        public string AlmacenId { get; set; }

        [Display(Name = "Producto")]
        public string ProductoId { get; set; }

        [Display(Name = "Unidad Administrativa")]
        public string UnidadAdministrativaId { get; set; }

        [Display(Name = "Proyecto")]
        public string ProyectoId { get; set; }
        
        [Display(Name = "Fuente de Financiamiento")]
        public string FuenteFinanciamientoId { get; set; }
    }
}