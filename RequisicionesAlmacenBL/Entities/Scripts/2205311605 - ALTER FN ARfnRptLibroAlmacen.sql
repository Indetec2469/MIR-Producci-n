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
CREATE OR ALTER FUNCTION [dbo].[ARfnRptLibroAlmacen]
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
		   proyecto.ProyectoId + ' - ' + proyecto.Nombre AS Proyecto,
		   ramo.RamoId + ' - ' + ramo.Nombre AS FuenteFinanciamiento,
		   dependencia.DependenciaId + ' - ' + dependencia.Nombre AS UnidadAdministrativa,
		   tipoGasto.TipoGastoId + ' - ' + tipoGasto.NombreCorto AS TipoGasto,
		   cpe.CuentaPresupuestalEgrId AS CuentaPresupuestalId,
		   um.Descripcion AS UnidadMedida,
		   Cantidad AS Existencia
	FROM ARtblAlmacenProducto AS ap
		 INNER JOIN tblAlmacen AS almacen ON ap.AlmacenId = almacen.AlmacenId AND almacen.AlmacenId = @almacenId
		 INNER JOIN tblProducto AS producto ON producto.ProductoId = ap.ProductoId
		 INNER JOIN tblUnidadDeMedida AS um ON um.UnidadDeMedidaId = producto.UnidadDeMedidaId
		 INNER JOIN tblCuentaPresupuestalEgr AS cpe ON cpe.CuentaPresupuestalEgrId = ap.CuentaPresupuestalId
		 INNER JOIN tblProyecto AS proyecto ON proyecto.ProyectoId = cpe.ProyectoId
		 INNER JOIN tblRamo AS ramo ON ramo.RamoId = cpe.RamoId
		 INNER JOIN tblDependencia AS dependencia ON dependencia.DependenciaId = cpe.DependenciaId
		 INNER JOIN tblTipoGasto AS tipoGasto ON tipoGasto.TipoGastoId = cpe.TipoGastoId
    WHERE ap.Borrado = 0
)
GO