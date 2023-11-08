SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspConsultaOrdenCompraProductos]
	@almacenId NVARCHAR(4),
	@dependenciaId NVARCHAR(10),	
	@proyectoId NVARCHAR(10),
	@ramoId NVARCHAR(10)
AS
-- ============================================================
-- Author:		Javier Elías
-- Create date: 16/08/2021
-- Modified date: 
-- Description:	Procedimiento para Obtener los registros que se van a
--						agregar al combo de Productos de la Ficha de OC.
-- ============================================================
SELECT almacenProducto.AlmacenProductoId,
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
FROM ARtblAlmacenProducto AS almacenProducto
     INNER JOIN tblProducto AS producto ON almacenProducto.ProductoId = producto.ProductoId AND producto.Status = 'A'
     INNER JOIN tblUnidadDeMedida AS um ON producto.UnidadDeMedidaId = um.UnidadDeMedidaId
     INNER JOIN tblCuentaPresupuestalEgr AS cuenta ON almacenProducto.CuentaPresupuestalId = cuenta.CuentaPresupuestalEgrId
     INNER JOIN tblDependencia AS unidadAdministrativa ON cuenta.DependenciaId = unidadAdministrativa.DependenciaId AND unidadAdministrativa.DependenciaId = ISNULL(@dependenciaId, unidadAdministrativa.DependenciaId)
     INNER JOIN tblProyecto AS proyecto ON cuenta.ProyectoId = proyecto.ProyectoId AND proyecto.ProyectoId = ISNULL(@proyectoId, proyecto.ProyectoId)
	 INNER JOIN tblRamo AS fuenteFinanciamiento ON cuenta.RamoId = fuenteFinanciamiento.RamoId AND fuenteFinanciamiento.RamoId = ISNULL(@ramoId, fuenteFinanciamiento.RamoId)
     INNER JOIN tblTipoGasto AS tipoGasto ON cuenta.TipoGastoId = tipoGasto.TipoGastoId
WHERE almacenProducto.Borrado = 0
      AND almacenProducto.AlmacenId = @almacenId
ORDER BY producto.ProductoId,
      unidadAdministrativa.DependenciaId,
	  proyecto.ProyectoId,
	  fuenteFinanciamiento.RamoId,
	  tipoGasto.TipoGastoId
GO