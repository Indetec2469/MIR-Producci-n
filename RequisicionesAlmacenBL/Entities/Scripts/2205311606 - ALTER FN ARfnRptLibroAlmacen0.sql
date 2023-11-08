SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Alonso Soto
-- Create date: 21/07/2021
-- Modified: Javier Elías
-- Modified date: 31/05/2022
-- Description:	Funcion para obtener el reporte de 
--              Impresion de Inventario
-- =============================================
CREATE OR ALTER FUNCTION [dbo].[ARfnRptLibroAlmacen0] 
(	
	@almacenId VARCHAR(4)
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT almacen.Nombre AS Almacen,
		   producto.ProductoId AS ProductoId,
		   producto.Descripcion AS Descripcion,
		   um.Descripcion AS UnidadMedida,
		   SUM(Cantidad) AS Existencia
	FROM ARtblAlmacenProducto AS ap
		 INNER JOIN tblAlmacen AS almacen ON ap.AlmacenId = almacen.AlmacenId AND almacen.AlmacenId = @almacenId
		 INNER JOIN tblProducto AS producto ON ap.ProductoId = producto.ProductoId
		 INNER JOIN tblUnidadDeMedida AS um ON producto.UnidadDeMedidaId = um.UnidadDeMedidaId
	WHERE ap.Borrado = 0
	GROUP BY almacen.Nombre,
		   producto.ProductoId,
		   producto.Descripcion,
		   um.Descripcion	
)
GO