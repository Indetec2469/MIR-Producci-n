SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ================================================================
-- Author:		Javier Elías
-- Create date: 01/03/2021
-- Modified date: 07/12/2021
-- Description:	Procedimiento para Obtener los registros que se van a mostrar
--						en el combo de productos de la Ficha Inventario Ajuste.
-- ================================================================
CREATE OR ALTER PROCEDURE [dbo].[ARspListadoProductosPorAlmacenId]
	@almacenId VARCHAR(4)
AS
SELECT AlmacenProductoId,
       fuenteFinanciamiento.RamoId AS FuenteFinanciamientoId,
	   fuenteFinanciamiento.Nombre AS FuenteFinanciamiento,
       proyecto.ProyectoId,
       proyecto.Nombre AS Proyecto,
       unidadAdministrativa.DependenciaId AS UnidadAdministrativaId,
       UnidadAdministrativa.Nombre AS UnidadAdministrativa,
	   tipoGasto.TipoGastoId,
       tipoGasto.Nombre AS TipoGasto,
       producto.ProductoId,
	   producto.Descripcion,
       producto.ProductoId + ' - ' + producto.Descripcion AS Producto,
       um.UnidadDeMedidaId,
       um.Descripcion AS UnidadDeMedida,
       CuentaPresupuestalId,
       CostoPromedio,
       Cantidad AS ExistenciaActual
FROM tblProducto AS producto
     INNER JOIN ARtblAlmacenProducto AS almacen ON producto.ProductoId = almacen.ProductoId AND AlmacenId = @almacenId AND Borrado = 0
     INNER JOIN tblCuentaPresupuestalEgr AS cuenta ON almacen.CuentaPresupuestalId = cuenta.CuentaPresupuestalEgrId
     INNER JOIN tblRamo AS fuenteFinanciamiento ON cuenta.RamoId = fuenteFinanciamiento.RamoId
     INNER JOIN tblProyecto AS proyecto ON cuenta.ProyectoId = proyecto.ProyectoId
     INNER JOIN tblDependencia AS unidadAdministrativa ON cuenta.DependenciaId = unidadAdministrativa.DependenciaId
     INNER JOIN tblTipoGasto AS tipoGasto ON cuenta.TipoGastoId = tipoGasto.TipoGastoId
     INNER JOIN tblUnidadDeMedida AS um ON producto.UnidadDeMedidaId = um.UnidadDeMedidaId
ORDER BY producto.ProductoId
GO