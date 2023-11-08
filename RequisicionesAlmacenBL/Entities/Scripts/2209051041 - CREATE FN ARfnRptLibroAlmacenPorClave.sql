DROP FUNCTION IF EXISTS ARfnRptLibroAlmacen
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
CREATE OR ALTER FUNCTION [dbo].[ARfnRptLibroAlmacenPorClave]
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
		   proyecto.ProyectoId+' - '+proyecto.Nombre AS Proyecto,
		   ramo.RamoId+' - '+ramo.Nombre AS FuenteFinanciamiento,
		   dependencia.DependenciaId+' - '+dependencia.Nombre AS UnidadAdministrativa,
		   tipoGasto.TipoGastoId+' - '+tipoGasto.NombreCorto AS TipoGasto,
		   cpe.CuentaPresupuestalEgrId AS CuentaPresupuestalId,
		   um.Descripcion AS UnidadMedida,
		   SUM(detalles.Existencia) AS Existencia,
		   SUM(ISNULL(Conteo, 0.0)) AS Conteo
	FROM ARtblInventarioFisico AS inventario
		 INNER JOIN ARtblInventarioFisicoDetalle AS detalles ON inventario.InventarioFisicoId = detalles.InventarioFisicoId
		 INNER JOIN tblAlmacen AS almacen ON inventario.AlmacenId = almacen.AlmacenId
		 INNER JOIN tblProducto AS producto ON detalles.ProductoId = producto.ProductoId
		 INNER JOIN tblUnidadDeMedida AS um ON producto.UnidadDeMedidaId = um.UnidadDeMedidaId
		 INNER JOIN ARtblAlmacenProducto AS ap ON detalles.AlmacenProductoId = ap.AlmacenProductoId
		 INNER JOIN tblCuentaPresupuestalEgr AS cpe ON cpe.CuentaPresupuestalEgrId = ap.CuentaPresupuestalId
		 INNER JOIN tblProyecto AS proyecto ON proyecto.ProyectoId = cpe.ProyectoId
		 INNER JOIN tblRamo AS ramo ON ramo.RamoId = cpe.RamoId
		 INNER JOIN tblDependencia AS dependencia ON dependencia.DependenciaId = cpe.DependenciaId
		 INNER JOIN tblTipoGasto AS tipoGasto ON tipoGasto.TipoGastoId = cpe.TipoGastoId
	WHERE inventario.InventarioFisicoId = @inventarioId
	GROUP BY almacen.Nombre,
			 producto.ProductoId,
			 producto.Descripcion,
			 proyecto.ProyectoId,
			 proyecto.Nombre,
			 ramo.RamoId,
			 ramo.Nombre,
			 dependencia.DependenciaId,
			 dependencia.Nombre,
			 tipoGasto.TipoGastoId,
			 tipoGasto.NombreCorto,
			 cpe.CuentaPresupuestalEgrId,
			 um.Descripcion
)
GO