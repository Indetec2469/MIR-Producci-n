SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspConsultaTransferenciaProductos]
AS
-- ===============================================================
-- Author:		Javier Elías
-- Create date: 26/11/2021
-- Modified date: 
-- Description:	Procedimiento para Obtener los registros que se van a
--						agregar al combo de Productos de la Ficha de Transferencias
-- ===============================================================
SELECT CONVERT(BIT, 0) AS Seleccionado,
       almacenProducto.AlmacenProductoId,
	   cuenta.CuentaPresupuestalEgrId,
	   almacen.AlmacenId,
	   almacen.Nombre AS Almacen,
	   producto.ProductoId,
	   producto.Descripcion,
	   --producto.ProductoId + ' - ' + producto.Descripcion + ' - ' + um.Descripcion AS Producto,
	   producto.ProductoId + ' - ' + producto.Descripcion AS Producto,
	   almacenProducto.Cantidad,
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
FROM ARtblAlmacenProducto AS almacenProducto
     INNER JOIN tblProducto AS producto ON almacenProducto.ProductoId = producto.ProductoId AND producto.Status = 'A'
     INNER JOIN tblUnidadDeMedida AS um ON producto.UnidadDeMedidaId = um.UnidadDeMedidaId
     INNER JOIN tblCuentaPresupuestalEgr AS cuenta ON almacenProducto.CuentaPresupuestalId = cuenta.CuentaPresupuestalEgrId
     INNER JOIN tblDependencia AS unidadAdministrativa ON cuenta.DependenciaId = unidadAdministrativa.DependenciaId
     INNER JOIN tblProyecto AS proyecto ON cuenta.ProyectoId = proyecto.ProyectoId
	 INNER JOIN tblRamo AS fuenteFinanciamiento ON cuenta.RamoId = fuenteFinanciamiento.RamoId
     INNER JOIN tblTipoGasto AS tipoGasto ON cuenta.TipoGastoId = tipoGasto.TipoGastoId
	 INNER JOIN tblAlmacen AS almacen ON almacenProducto.AlmacenId = almacen.AlmacenId
WHERE almacenProducto.Borrado = 0
		AND almacenProducto.Cantidad > 0
ORDER BY almacenProducto.AlmacenId,
      producto.ProductoId,
	  unidadAdministrativa.DependenciaId,
	  proyecto.ProyectoId,
	  fuenteFinanciamiento.RamoId,
	  tipoGasto.TipoGastoId
GO