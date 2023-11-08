SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Javier Elías
-- Create date: 23/06/2022
-- Modified date: 22/07/2022
-- Description:	Funcion para obtener el reporte de 
--              Libro de Almacén acumulado
-- =============================================
CREATE OR ALTER FUNCTION [dbo].[ARfnRptLibroAlmacenAcumulado] ( @ObjetosGastoId VARCHAR(1000), @fecha VARCHAR(100) )
RETURNS TABLE 
AS
RETURN 
(
	SELECT CuentaAlmacenCodigo,
		   CuentaAlmacen,
		   producto.ProductoId,
		   producto.Descripcion,
		   SUM(ISNULL(movimiento.Cantidad, 0)) AS Cantidad,
		   movimiento.Descripcion AS UM,
		   ISNULL(movimiento.Costo, 0) AS CostoUnitario,
		   SUM(ISNULL(movimiento.Cantidad, 0) * ISNULL(movimiento.Costo, 0)) AS Monto,
		   dbo.GRfnGetFechaConFormatoCompleto(CONVERT(DATETIME, @fecha, 126)) AS FechaCorte
	FROM CGvwClasificadorObjetoGasto AS cuentas
		 INNER JOIN tblProducto AS producto ON LEFT(COGCodigo, 2) = LEFT(ObjetoGastoId, 2)
		 INNER JOIN tblUnidadDeMedida AS um ON producto.UnidadDeMedidaId = um.UnidadDeMedidaId
		 INNER JOIN ARtblAlmacenProducto AS ap ON producto.ProductoId = ap.ProductoId AND ap.Borrado = 0
		 CROSS APPLY
		 (
				SELECT TOP 1
					 CantidadAntesMovto + CantidadMovimiento AS Cantidad,
					 ISNULL(CostoUnitario, CostoPromedio) AS Costo,
					 um.Descripcion
				FROM ARtblInventarioMovimiento AS im
					 INNER JOIN tblUnidadDeMedida AS um ON im.UnidadMedidaId = um.UnidadDeMedidaId
				WHERE im.AlmacenProductoId = ap.AlmacenProductoId
					  AND im.FechaCreacion <= DATEADD(SECOND, 86399, CONVERT(DATETIME, @fecha, 126))
				ORDER BY FechaCreacion DESC
		 ) AS movimiento
	WHERE @ObjetosGastoId LIKE '%'+CuentaAlmacenCodigo+'%'
	GROUP BY CuentaAlmacenCodigo,
			 CuentaAlmacen,
			 producto.ProductoId,
			 producto.Descripcion,
			 movimiento.Descripcion,
			 ISNULL(movimiento.Costo, 0)
)
GO