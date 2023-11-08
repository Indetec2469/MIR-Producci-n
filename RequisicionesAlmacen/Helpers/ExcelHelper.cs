using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RequisicionesAlmacen.Helpers
{
    public class ExcelHelper
    {
        public void ExcelTitulo(ref ExcelRange excelRange)
        {
            excelRange.Merge = true;
            excelRange.Style.Font.Bold = true;
            excelRange.Style.Font.Size = 12;
            excelRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            excelRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        }

        public void ExcelParrafo(ref ExcelRange excelRange)
        {
            excelRange.Merge = true;
            excelRange.Style.WrapText = true;
            //excelRange.Style.Font.Bold = true;
            excelRange.Style.Font.Size = 8;
            excelRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            excelRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        }

        public void ExcelTablaTitulo(ref ExcelRange excelRange, bool esMerge, int size, bool esCentroHorizontal, bool esCentroVertical)
        {
            excelRange.Merge = esMerge;
            excelRange.Style.WrapText = true;
            excelRange.Style.Font.Bold = true;
            excelRange.Style.Font.Size = size;
            excelRange.Style.HorizontalAlignment = esCentroHorizontal ? ExcelHorizontalAlignment.Center : ExcelHorizontalAlignment.Left;
            excelRange.Style.VerticalAlignment = esCentroVertical ? ExcelVerticalAlignment.Center : ExcelVerticalAlignment.Top;
            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);
        }

        public void ExcelTablaParrafo(ref ExcelRange excelRange, bool esMerge, int size)
        {
            excelRange.Merge = esMerge;
            excelRange.Style.WrapText = true;
            excelRange.Style.Font.Size = size;
            excelRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            excelRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);
        }

        public void EncabezadoEnteStyle(ref ExcelRange excelRange)
        {
            ExcelCellStyle(ref excelRange, true, true, 12, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
        }

        public void EncabezadoEstadoStyle(ref ExcelRange excelRange)
        {
            ExcelCellStyle(ref excelRange, true, true, 11, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
        }

        public void EncabezadoTituloReporteStyle(ref ExcelRange excelRange)
        {
            ExcelCellStyle(ref excelRange, true, false, 10, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
        }

        public void EncabezadoFechaCorteStyle(ref ExcelRange excelRange)
        {
            ExcelCellStyle(ref excelRange, true, false, 8, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
        }

        public void EncabezadoDatosTituloStyle(ref ExcelRange excelRange)
        {
            ExcelCellStyle(ref excelRange, true, true, 8, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, false);
        }

        public void EncabezadoDatosStyle(ref ExcelRange excelRange)
        {
            ExcelCellStyle(ref excelRange, true, false, 8, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, false);
        }

        public void TablaColumnaStyle(ref ExcelRange excelRange, Nullable<ExcelHorizontalAlignment> horizontalAlignment, Nullable<ExcelVerticalAlignment> verticalAlignment)
        {
            ExcelCellStyle(ref excelRange, false, true, 8, horizontalAlignment, verticalAlignment, false);
        }

        public void TablaDatoStyle(ref ExcelRange excelRange, Nullable<ExcelHorizontalAlignment> horizontalAlignment, Nullable<ExcelVerticalAlignment> verticalAlignment)
        {
            ExcelCellStyle(ref excelRange, false, false, 7, horizontalAlignment, verticalAlignment, false);
        }

        private void ExcelCellStyle(ref ExcelRange excelRange, 
                                      bool esMerge,
                                      bool esBold,
                                      int size, 
                                      Nullable<ExcelHorizontalAlignment> horizontalAlignment, 
                                      Nullable<ExcelVerticalAlignment> verticalAlignment,
                                      bool agregarBorde)
        {
            excelRange.Merge = esMerge;
            excelRange.Style.WrapText = true;
            excelRange.Style.Font.Bold = esBold;
            excelRange.Style.Font.Size = size;
            excelRange.Style.HorizontalAlignment = horizontalAlignment != null ? horizontalAlignment.GetValueOrDefault() : ExcelHorizontalAlignment.Left;
            excelRange.Style.VerticalAlignment = verticalAlignment != null ? verticalAlignment.GetValueOrDefault() : ExcelVerticalAlignment.Top;
            excelRange.Style.Font.Name = "Arial";

            if (agregarBorde)
            {
                excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }
        }
    }
}