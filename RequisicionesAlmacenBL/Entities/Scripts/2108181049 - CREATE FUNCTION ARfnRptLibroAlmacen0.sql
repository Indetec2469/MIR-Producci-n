SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Alonso Soto
-- Create date: 21/07/2021
-- Description:	Funcion para obtener el reporte de 
--              Impresion de Inventario
-- =============================================
CREATE FUNCTION [dbo].[ARfnRptLibroAlmacen0] 
(	
	@almacenId VARCHAR(4)
)
RETURNS TABLE 
AS
RETURN 
(
	-- Add the SELECT statement with parameter references here
	SELECT tblProducto.ProductoId AS ProductoId,
		   tblProducto.Descripcion AS Descripcion,
		   tblUnidadDeMedida.Descripcion AS UnidadMedida
	FROM dbo.ARtblAlmacenProducto
	INNER JOIN dbo.tblProducto ON tblProducto.ProductoId = ARtblAlmacenProducto.ProductoId
	INNER JOIN dbo.tblUnidadDeMedida ON tblUnidadDeMedida.UnidadDeMedidaId = tblProducto.UnidadDeMedidaId
	WHERE dbo.ARtblAlmacenProducto.AlmacenId = @almacenId --AND
          --Cantidad > 0
	GROUP BY tblProducto.ProductoId,
		     tblProducto.Descripcion,
		     tblUnidadDeMedida.Descripcion
	--HAVING Cantidad > 0
	
)
GO


