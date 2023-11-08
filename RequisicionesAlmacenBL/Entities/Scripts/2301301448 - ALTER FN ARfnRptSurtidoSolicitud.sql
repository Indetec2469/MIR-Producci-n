SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ========================================================
-- Author:			Javier Elías
-- Create date:		20/08/2021
-- Modified date:	30/01/2023
-- Description:		Función para obtener el reporte de 
--					Surtimiento de Requisición Material.
-- ========================================================
CREATE OR ALTER FUNCTION [dbo].[ARfnRptSurtidoSolicitud]
(	
	@requisicionId INT,
	@agrupadorId INT
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT CodigoRequisicion AS Requisicion,
		   almacen.Nombre AS Almacen,
		   detalle.ProductoId,
		   detalle.Descripcion AS Producto,
		   um.Descripcion AS UnidadDeMedida,
		   ABS(surtimiento.CantidadMovimiento) AS Cantidad,
		   detalle.UnidadAdministrativaId AS UnidadAdministrativa,
		   detalle.ProyectoId AS Proyecto,
		   agrupador.InventarioMovtoAgrupadorId AS Salida,
		   FORMAT(agrupador.FechaCreacion, 'dd/MM/yyyy') AS Fecha,
		   NumeroOficio,
		   ISNULL(p.Poliza, '') AS Poliza,
		   Observaciones,
		   Almacenes = STUFF(
				(
					SELECT DISTINCT
							', ' + almacenSurtimiento.Nombre
					FROM ARtblRequisicionMaterialDetalle AS detalle
							INNER JOIN ARtblInventarioMovimiento AS surtimiento ON surtimiento.ReferenciaMovtoId = detalle.RequisicionMaterialDetalleId AND surtimiento.InventarioMovtoAgrupadorId = @agrupadorId AND TipoMovimientoId = 63 -- Requisición Material Surtimiento
							INNER JOIN ARtblAlmacenProducto AS ap ON surtimiento.AlmacenProductoId = ap.AlmacenProductoId
							INNER JOIN tblAlmacen AS almacenSurtimiento ON ap.AlmacenId = almacenSurtimiento.AlmacenId
					WHERE detalle.RequisicionMaterialId = @requisicionId
					ORDER BY ', ' + almacenSurtimiento.Nombre
					FOR XML PATH('')
				), 1, 1, '')
	FROM ARtblRequisicionMaterial AS requisicion
		 INNER JOIN ARtblRequisicionMaterialDetalle AS detalle ON requisicion.RequisicionMaterialId = detalle.RequisicionMaterialId
		 INNER JOIN tblUnidadDeMedida AS um ON detalle.UnidadMedidaId = um.UnidadDeMedidaId
		 INNER JOIN ARtblInventarioMovimiento AS surtimiento ON surtimiento.ReferenciaMovtoId = detalle.RequisicionMaterialDetalleId AND surtimiento.InventarioMovtoAgrupadorId = @agrupadorId AND TipoMovimientoId = 63 -- Requisición Material Surtimiento
		 INNER JOIN ARtblInventarioMovimientoAgrupador AS agrupador ON surtimiento.InventarioMovtoAgrupadorId = agrupador.InventarioMovtoAgrupadorId
		 INNER JOIN ARtblAlmacenProducto AS ap ON surtimiento.AlmacenProductoId = ap.AlmacenProductoId
		 INNER JOIN tblAlmacen AS almacen ON ap.AlmacenId = almacen.AlmacenId
		 LEFT JOIN tblPolizaRef AS pr ON agrupador.InventarioMovtoAgrupadorId = pr.Referencia AND pr.Tipo = 'A' AND pr.TipoMovimientoId = 75 -- Surtimiento de Requisición
		 LEFT JOIN tblPoliza AS p ON pr.PolizaId = p.PolizaId
	WHERE detalle.RequisicionMaterialId = @requisicionId
)
GO