SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Javier Elías
-- Create date: 23/06/2022
-- Description:	Funcion para obtener el reporte de 
--              Libro de Almacén acumulado
-- =============================================
CREATE OR ALTER FUNCTION [dbo].[ARfnRptLibroAlmacenAcumulado] ( @ObjetosGastoId VARCHAR(1000) )
RETURNS TABLE 
AS
RETURN 
(
	SELECT CuentaAlmacenCodigo,
		   CuentaAlmacen,
		   producto.ProductoId,
		   producto.Descripcion,
		   SUM(ISNULL(Cantidad, 0)) AS Cantidad,
		   um.Descripcion AS UM,
		   ISNULL(ISNULL(CostoPromedio, CostoUltimo), 0) AS CostoUnitario,
		   SUM(ISNULL(Cantidad, 0)) * ISNULL(ISNULL(CostoPromedio, CostoUltimo), 0) AS Monto
	FROM CGvwClasificadorObjetoGasto AS cuentas
		 INNER JOIN tblProducto AS producto ON LEFT(COGCodigo, 2) = LEFT(ObjetoGastoId, 2)
		 INNER JOIN tblUnidadDeMedida AS um ON producto.UnidadDeMedidaId = um.UnidadDeMedidaId
		 INNER JOIN ARtblAlmacenProducto AS ap ON producto.ProductoId = ap.ProductoId AND ap.Borrado = 0
	WHERE @ObjetosGastoId LIKE '%' + CuentaAlmacenCodigo + '%'
	GROUP BY CuentaAlmacenCodigo,
			 CuentaAlmacen,
			 producto.ProductoId,
			 producto.Descripcion,
			 um.Descripcion,
			 ISNULL(ISNULL(CostoPromedio, CostoUltimo), 0)
)
GO