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
ALTER FUNCTION ARfnRptLibroAlmacen 
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
		   dbo.tblProyecto.Nombre AS Proyecto,
		   dbo.tblRamo.Nombre AS FuenteFinanciamiento,
		   dbo.tblDependencia.Nombre AS UnidadAdministrativa,
		   dbo.tblTipoGasto.NombreCorto AS TipoGasto,
		   dbo.tblCuentaPresupuestalEgr.CuentaPresupuestalEgrId AS CuentaPresupuestalId,
		   tblUnidadDeMedida.Descripcion AS UnidadMedida,
		   Cantidad AS Existencia
	FROM dbo.ARtblAlmacenProducto
	INNER JOIN dbo.tblProducto ON tblProducto.ProductoId = ARtblAlmacenProducto.ProductoId
	INNER JOIN dbo.tblUnidadDeMedida ON tblUnidadDeMedida.UnidadDeMedidaId = tblProducto.UnidadDeMedidaId
	INNER JOIN dbo.tblCuentaPresupuestalEgr ON tblCuentaPresupuestalEgr.CuentaPresupuestalEgrId = ARtblAlmacenProducto.CuentaPresupuestalId
	INNER JOIN dbo.tblProyecto ON tblProyecto.ProyectoId = tblCuentaPresupuestalEgr.ProyectoId
	INNER JOIN dbo.tblRamo ON tblRamo.RamoId = tblCuentaPresupuestalEgr.RamoId
	INNER JOIN dbo.tblDependencia ON tblDependencia.DependenciaId = tblCuentaPresupuestalEgr.DependenciaId
	INNER JOIN dbo.tblTipoGasto ON tblTipoGasto.TipoGastoId = tblCuentaPresupuestalEgr.TipoGastoId
	WHERE dbo.ARtblAlmacenProducto.AlmacenId = @almacenId --AND
          --Cantidad > 0
	--GROUP BY tblProducto.ProductoId,
	--	     tblProducto.Descripcion,
	--	     tblUnidadDeMedida.Descripcion,
	--		 tblProyecto.Nombre,
	--		 tblRamo.Nombre,
	--		 tblDependencia.Nombre,
	--		 tblTipoGasto.NombreCorto,
	--		 dbo.tblCuentaPresupuestalEgr.CuentaPresupuestalEgrId
	--HAVING Cantidad > 0
	
)
GO
