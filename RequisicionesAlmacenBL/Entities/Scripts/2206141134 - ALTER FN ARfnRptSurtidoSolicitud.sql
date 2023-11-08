SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ========================================================
-- Author:		Javier Elías
-- Create date: 20/08/2021
-- Modified date: 14/06/2022
-- Description:	Función para obtener el reporte de Surtimiento
--						de Requisición Material.
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
		   ProductoId,
		   detalle.Descripcion AS Producto,
		   um.Descripcion AS UnidadDeMedida,
		   ABS(surtimiento.CantidadMovimiento) AS Cantidad,
		   detalle.UnidadAdministrativaId AS UnidadAdministrativa,
		   detalle.ProyectoId AS Proyecto,
		   agrupador.InventarioMovtoAgrupadorId AS Salida,
		   FORMAT(agrupador.FechaCreacion, 'dd/MM/yyyy') AS Fecha,
		   NumeroOficio,
		   Observaciones
	FROM ARtblRequisicionMaterial AS requisicion
		 INNER JOIN ARtblRequisicionMaterialDetalle AS detalle ON requisicion.RequisicionMaterialId = detalle.RequisicionMaterialId
		 INNER JOIN tblAlmacen AS almacen ON detalle.AlmacenId = almacen.AlmacenId
		 INNER JOIN tblUnidadDeMedida AS um ON detalle.UnidadMedidaId = um.UnidadDeMedidaId
		 INNER JOIN ARtblInventarioMovimiento AS surtimiento ON surtimiento.ReferenciaMovtoId = detalle.RequisicionMaterialDetalleId AND surtimiento.InventarioMovtoAgrupadorId = @agrupadorId AND TipoMovimientoId = 63 -- Requisición Material Surtimiento
		 INNER JOIN ARtblInventarioMovimientoAgrupador AS agrupador ON surtimiento.InventarioMovtoAgrupadorId = agrupador.InventarioMovtoAgrupadorId
	WHERE detalle.RequisicionMaterialId = @requisicionId
)
GO