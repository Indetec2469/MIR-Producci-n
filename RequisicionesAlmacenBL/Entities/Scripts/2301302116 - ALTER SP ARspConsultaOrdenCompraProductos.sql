SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspConsultaOrdenCompraProductos]
	@almacenId NVARCHAR(4),
	@dependenciaId NVARCHAR(10),
	@proyectoId NVARCHAR(10),
	@ramoId NVARCHAR(10),
	@productoId VARCHAR(10)
AS
-- ============================================================
-- Author:			Javier Elías
-- Create date:		16/08/2021
-- Modified date:	30/01/2023
-- Description:		Procedimiento para Obtener los registros
--					que se van a agregar al combo de Productos
--					de la Ficha de OC.
-- ============================================================
SELECT ISNULL(almacenProducto.AlmacenProductoId, -1 * ROW_NUMBER() OVER (ORDER BY producto.ProductoId, unidadAdministrativa.DependenciaId, proyecto.ProyectoId, fuenteFinanciamiento.RamoId, tipoGasto.TipoGastoId)) AS AlmacenProductoId,
       cuenta.CuentaPresupuestalEgrId,
	   producto.ProductoId,
	   producto.Descripcion,
	   producto.ProductoId + ' - ' + producto.Descripcion + ' - ' + um.Descripcion AS Producto,
       um.UnidadDeMedidaId,
       um.Descripcion AS UnidadDeMedida,
       CostoPromedio AS Costo,
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
     LEFT JOIN ARtblAlmacenProducto AS almacenProducto ON producto.ProductoId = almacenProducto.ProductoId
                                                          AND cuenta.CuentaPresupuestalEgrId = almacenProducto.CuentaPresupuestalId
                                                          AND almacenProducto.Borrado = 0
														  AND almacenProducto.AlmacenId = @almacenId
WHERE producto.Status = 'A'
      AND LEFT(producto.ProductoId, 1) IN ('2', '5')
	  AND (ISNULL(@dependenciaId, '') = '' OR unidadAdministrativa.DependenciaId = @dependenciaId)
      AND (ISNULL(@proyectoId, '') = '' OR proyecto.ProyectoId = @proyectoId)
      AND (ISNULL(@ramoId, '') = '' OR fuenteFinanciamiento.RamoId = @ramoId)
	  AND (ISNULL(@productoId, '') = '' OR producto.ProductoId = @productoId)
ORDER BY producto.ProductoId,
         unidadAdministrativa.DependenciaId,
         proyecto.ProyectoId,
         fuenteFinanciamiento.RamoId,
         tipoGasto.TipoGastoId
GO