SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ========================================================
-- Author:		Javier Elías
-- Create date: 01/10/2021
-- Modified date: 
-- Description:	Función para obtener el reporte de 
--						Existencias por Partidas Específicas
-- ========================================================
CREATE OR ALTER FUNCTION [dbo].[ARfnRptExistenciasPartidas]
(	
	@objetoGastoId VARCHAR (8),
	@usuarioId INT
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT dbo.RHfnGetNombreCompletoEmpleado(usuarioImpresion.EmpleadoId) AS Usuario,
			'rptExistenciasPartidas' AS Reporte,
			dbo.GRfnGetFechaConFormato(GETDATE(), 0) AS FechaImpresion,
			REPLACE(dbo.GRfnGetFechaConFormato(GETDATE(), 1), (dbo.GRfnGetFechaConFormato(GETDATE(), 0)+' '), '') AS HoraImpresion,
			producto.ObjetoGastoId,
			producto.ProductoId AS Codigo,
			producto.Descripcion,
			um.Descripcion AS UM,
			SUM(ap.Cantidad) AS Existencia,
			producto.StockMinimo AS Minimo,
			producto.StockMaximo AS Maximo,
			CASE WHEN SUM(ap.Cantidad) BETWEEN producto.StockMinimo AND producto.StockMaximo THEN 0
					ELSE CASE WHEN SUM(ap.Cantidad) < producto.StockMinimo THEN producto.StockMinimo - SUM(ap.Cantidad) 
					ELSE CASE WHEN SUM(ap.Cantidad) > producto.StockMaximo THEN producto.StockMaximo - SUM(ap.Cantidad) 
			END END END AS Diferencia
	FROM tblProducto AS producto
			INNER JOIN tblUnidadDeMedida AS um ON producto.UnidadDeMedidaId = um.UnidadDeMedidaId
			INNER JOIN ARtblAlmacenProducto AS ap ON producto.ProductoId = ap.ProductoId
													AND ap.Borrado = 0
			INNER JOIN tblCuentaPresupuestalEgr AS cp ON ap.CuentaPresupuestalId = cp.CuentaPresupuestalEgrId
			INNER JOIN GRtblUsuario AS usuarioImpresion ON usuarioImpresion.UsuarioId = @usuarioId
	WHERE producto.Status = 'A' -- Activos
			AND producto.ObjetoGastoId = ISNULL(@objetoGastoId, producto.ObjetoGastoId)
	GROUP BY producto.ObjetoGastoId,
				producto.ProductoId,
				producto.Descripcion,
				um.Descripcion,
				producto.StockMinimo,
				producto.StockMaximo,
				usuarioImpresion.EmpleadoId
)
GO