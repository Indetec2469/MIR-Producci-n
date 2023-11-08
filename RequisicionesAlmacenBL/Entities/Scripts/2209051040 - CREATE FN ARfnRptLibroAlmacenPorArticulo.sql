DROP FUNCTION IF EXISTS ARfnRptLibroAlmacen0
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Alonso Soto
-- Create date: 21/07/2021
-- Modified: Javier Elías
-- Modified date: 05/09/2022
-- Description:	Funcion para obtener el reporte de 
--              Impresion de Inventario
-- =============================================
CREATE OR ALTER FUNCTION [dbo].[ARfnRptLibroAlmacenPorArticulo] 
(	
	@inventarioId VARCHAR(4)
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT almacen.Nombre AS Almacen,
		   producto.ProductoId AS ProductoId,
		   producto.Descripcion AS Descripcion,
		   um.Descripcion AS UnidadMedida,
		   SUM(detalles.Existencia) AS Existencia,
		   SUM(ISNULL(Conteo, 0.0)) AS Conteo
	FROM ARtblInventarioFisico AS inventario
		 INNER JOIN ARtblInventarioFisicoDetalle AS detalles ON inventario.InventarioFisicoId = detalles.InventarioFisicoId
		 INNER JOIN tblAlmacen AS almacen ON inventario.AlmacenId = almacen.AlmacenId
		 INNER JOIN tblProducto AS producto ON detalles.ProductoId = producto.ProductoId
		 INNER JOIN tblUnidadDeMedida AS um ON producto.UnidadDeMedidaId = um.UnidadDeMedidaId
	WHERE inventario.InventarioFisicoId = @inventarioId
	GROUP BY almacen.Nombre,
			 producto.ProductoId,
			 producto.Descripcion,
			 um.Descripcion
)
GO