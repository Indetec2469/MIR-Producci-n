SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspConsultaTransferenciaProductosDestino] @almacenId VARCHAR(10), @productoId VARCHAR(20)
AS
-- ===============================================================
-- Author:			Javier Elías
-- Create date:		26/01/2023
-- Modified date:	
-- Description:		Procedimiento para Obtener los registros 
--					que se van a agregar al combo de Productos
--					Destino de la Ficha de Transferencias
-- ===============================================================
SELECT CONVERT(BIT, 0) AS Seleccionado,
       ISNULL(almacenProducto.AlmacenProductoId, -1 * ROW_NUMBER() OVER (ORDER BY unidadAdministrativa.DependenciaId, proyecto.ProyectoId, fuenteFinanciamiento.RamoId, tipoGasto.TipoGastoId)) AS AlmacenProductoId,
	   cuenta.CuentaPresupuestalEgrId,
       almacen.AlmacenId,
       almacen.Nombre AS Almacen,
       producto.ProductoId,
       producto.Descripcion,
       producto.ProductoId+' - '+producto.Descripcion AS Producto,
       ISNULL(almacenProducto.Cantidad, 0) AS Cantidad,
       um.UnidadDeMedidaId AS UnidadMedidaId,
       um.Descripcion AS UnidadDeMedida,
       unidadAdministrativa.DependenciaId AS UnidadAdministrativaId,
       unidadAdministrativa.Nombre AS UnidadAdministrativa,
       proyecto.ProyectoId,
       proyecto.Nombre AS Proyecto,
       fuenteFinanciamiento.RamoId AS FuenteFinanciamientoId,
       fuenteFinanciamiento.Nombre AS FuenteFinanciamiento,
       tipoGasto.TipoGastoId,
       tipoGasto.Nombre AS TipoGasto
FROM tblProducto AS producto
     INNER JOIN tblUnidadDeMedida AS um ON producto.UnidadDeMedidaId = um.UnidadDeMedidaId
     INNER JOIN tblCuentaPresupuestalEgr AS cuenta ON producto.ObjetoGastoId = cuenta.ObjetoGastoId
     INNER JOIN tblDependencia AS unidadAdministrativa ON cuenta.DependenciaId = unidadAdministrativa.DependenciaId
     INNER JOIN tblProyecto AS proyecto ON cuenta.ProyectoId = proyecto.ProyectoId
     INNER JOIN tblRamo AS fuenteFinanciamiento ON cuenta.RamoId = fuenteFinanciamiento.RamoId
     INNER JOIN tblTipoGasto AS tipoGasto ON cuenta.TipoGastoId = tipoGasto.TipoGastoId
     INNER JOIN tblAlmacen AS almacen ON almacen.AlmacenId = @almacenId
     LEFT JOIN ARtblAlmacenProducto AS almacenProducto ON producto.ProductoId = almacenProducto.ProductoId
                                                          AND almacen.AlmacenId = almacenProducto.AlmacenId
                                                          AND cuenta.CuentaPresupuestalEgrId = almacenProducto.CuentaPresupuestalId
                                                          AND almacenProducto.Borrado = 0
WHERE producto.ProductoId = @productoId
      AND producto.Status = 'A'
ORDER BY unidadAdministrativa.DependenciaId,
         proyecto.ProyectoId,
         fuenteFinanciamiento.RamoId,
         tipoGasto.TipoGastoId
GO