SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspConsultaRequisicionPorSurtirDetalles]
	@requisicionId INT
AS
/* ****************************************************************
 * ARspConsultaRequisicionPorSurtirDetalles
 * ****************************************************************
 * Descripción: Procedimiento para Obtener los detalles de una Requisición
 *						 Pendiente por Surtir de la Ficha Requisición Material.
 *
 * autor: 	Javier Elías
 * Fecha: 	23.06.2021
 * Versión: 1.0.0
 *****************************************************************
 * PARAMETROS DE ENTRADA: RequisicionId
 * PARAMETROS DE SALIDA:
 *****************************************************************
*/ 
SELECT RequisicionMaterialDetalleId,
       detalle.RequisicionMaterialId,
	   ProductoId,
       detalle.Descripcion AS Producto,
       um.UnidadDeMedidaId,
       um.Descripcion AS UnidadDeMedida,
       CostoUnitario,
       almacen.AlmacenId,
       almacen.Nombre AS Almacen,
       UnidadAdministrativaId,
       unidadAdministrativa.Nombre AS UnidadAdministrativa,
       detalle.ProyectoId,
       proyecto.Nombre AS Proyecto,
       tipoGasto.TipoGastoId,
       tipoGasto.Nombre AS TipoGasto,
       Cantidad,
	   Existencia,
       ISNULL(Surtida, 0) AS Surtida,
	   Cantidad - ISNULL(Surtida, 0) AS PorSurtir,
	   CONVERT(DECIMAL(28, 10), NULL) AS CantidadSurtir,
       Comentarios,
       detalle.EstatusId,
	   estatus.Valor AS Estatus,
	   CONVERT(BIT, CASE WHEN detalle.EstatusId = 25 THEN 1 ELSE 0 END) AS Revision,
	   CONVERT(BIT, CASE WHEN detalle.EstatusId = 62 THEN 1 ELSE 0 END) AS PorComprar
FROM ARtblRequisicionMaterialDetalle AS detalle
     INNER JOIN tblUnidadDeMedida AS um ON detalle.UnidadMedidaId = um.UnidadDeMedidaId
	 INNER JOIN tblAlmacen AS almacen ON detalle.AlmacenId  = almacen.AlmacenId
	 INNER JOIN tblDependencia AS unidadAdministrativa ON detalle.UnidadAdministrativaId = unidadAdministrativa.DependenciaId
	 INNER JOIN tblProyecto AS proyecto ON detalle.ProyectoId = proyecto.ProyectoId
	 INNER JOIN tblTipoGasto AS tipoGasto ON detalle.TipoGastoId = tipoGasto.TipoGastoId
	 INNER JOIN GRtblControlMaestro AS estatus ON detalle.EstatusId = estatus.ControlId
	 CROSS APPLY 
	 (
			SELECT SUM(Cantidad) AS Existencia
			FROM ARtblAlmacenProducto AS almacenProducto
				 INNER JOIN tblCuentaPresupuestalEgr AS cuentaPresupuestal ON almacenProducto.CuentaPresupuestalId = cuentaPresupuestal.CuentaPresupuestalEgrId
			WHERE almacenProducto.AlmacenId = detalle.AlmacenId
				  AND almacenProducto.ProductoId = detalle.ProductoId
				  AND cuentaPresupuestal.DependenciaId = detalle.UnidadAdministrativaId
				  AND cuentaPresupuestal.ProyectoId = detalle.ProyectoId
				  AND cuentaPresupuestal.TipoGastoId = detalle.TipoGastoId
				  AND almacenProducto.Borrado = 0
	  ) AS existencia
	  OUTER APPLY 
	  (
			SELECT SUM(CantidadMovimiento) AS Surtida
			FROM ARtblInventarioMovimiento
			WHERE TipoMovimientoId = 63 -- Requisición Material Surtimiento
				  AND ReferenciaMovtoId = detalle.RequisicionMaterialDetalleId
	  ) AS surtimiento
WHERE detalle.RequisicionMaterialId = @requisicionId
      AND detalle.EstatusId IN (25, 26, 27, 62) -- Revisión, Pendiente por surtir, Surtido parcial, Por Comprar
GO