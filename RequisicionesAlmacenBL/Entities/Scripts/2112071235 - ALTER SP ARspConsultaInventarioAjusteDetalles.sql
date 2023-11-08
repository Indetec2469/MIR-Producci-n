SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================================
-- Author:		Javier Elías
-- Create date: 25/02/2021
-- Modified date: 07/12/2021
-- Description:	Procedimiento para Obtener los registros que se van a
--						agregar a los detalles de la Ficha Ajuste de Inventario.
-- ============================================================
CREATE OR ALTER PROCEDURE [dbo].[ARspConsultaInventarioAjusteDetalles]
	@inventarioAjusteId INT
AS

SELECT InventarioAjusteDetalleId,
       inventario.InventarioAjusteId,
       AlmacenProductoId,
	   almacen.AlmacenId,
       almacen.Nombre AS Almacen,
       FuenteFinanciamientoId,
       fuenteFinanciamiento.Nombre AS FuenteFinanciamiento,
       proyecto.ProyectoId,
       proyecto.Nombre AS Proyecto,
       UnidadAdministrativaId,
       UnidadAdministrativa.Nombre AS UnidadAdministrativa,
       detalle.TipoGastoId,
       tipoGasto.Nombre AS TipoGasto,
       detalle.ProductoId,
	   detalle.Descripcion,
       detalle.ProductoId + ' - ' + detalle.Descripcion AS Producto,
       UnidadDeMedidaId,
       um.Descripcion AS UnidadDeMedida,
       detalle.CuentaPresupuestalId,
       CostoUnitario AS CostoPromedio,
       detalle.TipoMovimientoId,
       tipoMovimiento.Valor AS TipoMovimiento,
       ConceptoAjusteId,
       ConceptoAjuste,
       detalle.Cantidad,
       ExistenciaActual,
       ExistenciaFinal,
	   detalle.Cantidad * CostoUnitario * CASE WHEN detalle.TipoMovimientoId = 10 THEN -1 ELSE 1 END AS CostoMovimiento,
       Comentarios,
	   ArchivoId
FROM ARtblInventarioAjuste AS inventario
     INNER JOIN ARtblInventarioAjusteDetalle AS detalle ON inventario.InventarioAjusteId = detalle.InventarioAjusteId
	 INNER JOIN ARtblAlmacenProducto AS almacenProducto ON detalle.AlmacenId = almacenProducto.AlmacenId AND detalle.ProductoId = almacenProducto.ProductoId AND detalle.CuentaPresupuestalId = almacenProducto.CuentaPresupuestalId
     INNER JOIN tblAlmacen AS almacen ON detalle.AlmacenId = almacen.AlmacenId
     INNER JOIN tblRamo AS fuenteFinanciamiento ON FuenteFinanciamientoId = RamoId
     INNER JOIN tblProyecto AS proyecto ON detalle.ProyectoId = proyecto.ProyectoId
     INNER JOIN tblDependencia AS unidadAdministrativa ON detalle.UnidadAdministrativaId = unidadAdministrativa.DependenciaId
     INNER JOIN tblTipoGasto AS tipoGasto ON detalle.TipoGastoId = tipoGasto.TipoGastoId
     INNER JOIN tblUnidadDeMedida AS um ON detalle.UnidadMedidaId = um.UnidadDeMedidaId
     INNER JOIN GRtblControlMaestro AS tipoMovimiento ON detalle.TipoMovimientoId = tipoMovimiento.ControlId
     INNER JOIN ARtblControlMaestroConceptoAjusteInventario AS conceptoAjuste ON detalle.ConceptoAjusteId = conceptoAjuste.ConceptoAjusteInventarioId
WHERE inventario.InventarioAjusteId = @inventarioAjusteId
GO


