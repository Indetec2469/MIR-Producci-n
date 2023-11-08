SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ========================================================
-- Author:		Javier Elías
-- Create date: 28/09/2021
-- Modified date: 
-- Description:	Función para obtener el reporte de Kardex
-- ========================================================
CREATE OR ALTER FUNCTION [dbo].[ARfnRptKardex]
(	
	@fechaInicio DATETIME,
	@fechaFin DATETIME,
	@tipoMvtoId INT,
	@mvtoId INT,
	@polizaId INT,
	@almacenId VARCHAR (4),
	@productoId VARCHAR (10),
	@unidadAdministrativaId VARCHAR (6),
	@proyectoId VARCHAR (6),
	@fuenteFinanciamientoId VARCHAR (6),
	@usuarioId INT
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT dbo.RHfnGetNombreCompletoEmpleado(usuarioImpresion.EmpleadoId) AS Usuario,
		   'rptKardex' AS Reporte,
		   dbo.GRfnGetFechaConFormato(GETDATE(), 0) AS FechaImpresion,
		   REPLACE(dbo.GRfnGetFechaConFormato(GETDATE(), 1), (dbo.GRfnGetFechaConFormato(GETDATE(), 0) + ' '), '') AS HoraImpresion,
		   almacen.AlmacenId,
		   almacen.Nombre AS Almacen,
		   dbo.GRfnGetFechaConFormato(ag.FechaCreacion, 1) AS Fecha,
		   tipoMvto.Valor AS TipoMovimiento,
		   ag.ReferenciaMovtoId,
		   MotivoMovto,
		   ISNULL(Poliza, 'N/A') AS Poliza,
		   producto.ProductoId,
		   producto.Descripcion AS Producto,
		   DependenciaId AS UA,
		   ProyectoId AS Proyecto,
		   RamoId AS FF,
		   um.Descripcion AS UM,
		   CASE WHEN CantidadMovimiento > 0 THEN CantidadMovimiento END AS Entrada,
		   CASE WHEN CantidadMovimiento < 0 THEN ABS(CantidadMovimiento) END AS Salida,
		   CantidadAntesMovto + CantidadMovimiento AS Existencia,
		   CostoUnitario,
		   ABS(CantidadMovimiento) * CostoUnitario AS Total,
		   im.CostoPromedio
	FROM ARtblInventarioMovimientoAgrupador AS ag
		 INNER JOIN ARtblInventarioMovimiento AS im ON ag.InventarioMovtoAgrupadorId = im.InventarioMovtoAgrupadorId
		 INNER JOIN ARtblAlmacenProducto AS ap ON im.AlmacenProductoId = ap.AlmacenProductoId 
                                                  AND ap.AlmacenId = ISNULL(@almacenId, ap.AlmacenId)
												  AND ap.ProductoId = ISNULL(@productoId, ap.ProductoId)
         INNER JOIN tblCuentaPresupuestalEgr AS cp ON ap.CuentaPresupuestalId = cp.CuentaPresupuestalEgrId
                                                      AND cp.DependenciaId = ISNULL(@unidadAdministrativaId, cp.DependenciaId)
													  AND cp.ProyectoId = ISNULL(@proyectoId, cp.ProyectoId)
													  AND cp.RamoId = ISNULL(@fuenteFinanciamientoId, cp.RamoId)
		 INNER JOIN tblAlmacen AS almacen ON ap.AlmacenId = almacen.AlmacenId
		 INNER JOIN tblProducto AS producto ON ap.ProductoId = producto.ProductoId
		 INNER JOIN tblUnidadDeMedida AS um ON im.UnidadMedidaId = um.UnidadDeMedidaId
		 INNER JOIN GRtblControlMaestro AS tipoMvto ON ag.TipoMovimientoId = tipoMvto.ControlId
		 LEFT JOIN tblPoliza AS poliza ON ag.PolizaId = poliza.PolizaId
		 INNER JOIN GRtblUsuario AS usuarioImpresion ON usuarioImpresion.UsuarioId = @usuarioId
	WHERE ag.FechaCreacion BETWEEN ISNULL(@fechaInicio, '19000101') AND ISNULL(DATEADD(SECOND, 86399, @fechaFin), '21001231')
		  AND ag.TipoMovimientoId = ISNULL(@tipoMvtoId, ag.TipoMovimientoId)
		  AND ag.ReferenciaMovtoId = ISNULL(@mvtoId, ag.ReferenciaMovtoId)
		  AND ISNULL(ag.PolizaId, 0) = ISNULL(@polizaId, 0)
)
GO