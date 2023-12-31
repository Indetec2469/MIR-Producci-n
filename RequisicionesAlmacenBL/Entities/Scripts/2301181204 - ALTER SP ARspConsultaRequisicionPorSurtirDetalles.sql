SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER   PROCEDURE [dbo].[ARspConsultaRequisicionPorSurtirDetalles]
	@requisicionId INT
AS
-- =======================================================================
-- Author:		Javier Elías
-- Create date: 23/06/2021
-- Modified date: 18/01/2023
-- Description:	Procedimiento para Obtener los detalles de una Requisición
--						Pendiente por Surtir de la Ficha Requisición Material.
-- =======================================================================
SELECT RequisicionMaterialDetalleId,
       detalle.RequisicionMaterialId,
	   ProductoId,
       detalle.Descripcion AS Producto,
       um.UnidadDeMedidaId,
       um.Descripcion AS UnidadDeMedida,
       CostoUnitario,
       detalle.UnidadAdministrativaId,
       unidadAdministrativa.Nombre AS UnidadAdministrativa,
       detalle.ProyectoId,
       proyecto.Nombre AS Proyecto,
       tipoGasto.TipoGastoId,
       tipoGasto.Nombre AS TipoGasto,
       Cantidad,
	   Existencia,
       -1 * ISNULL(Surtida, 0) AS Surtida,
	   Cantidad + ISNULL(Surtida, 0) AS PorSurtir,
	   CONVERT(DECIMAL(28, 10), NULL) AS CantidadSurtir,
       Comentarios,
       detalle.EstatusId,
	   estatus.Valor AS Estatus,
	   CONVERT(BIT, CASE WHEN detalle.EstatusId = 88 THEN 1 ELSE 0 END) AS Revision,
	   CONVERT(BIT, CASE WHEN detalle.EstatusId = 83 THEN 1 ELSE 0 END) AS PorComprar,
	   CONVERT(VARCHAR(2000), NULL) AS Motivo
FROM ARtblRequisicionMaterialDetalle AS detalle
	 INNER JOIN ARtblRequisicionMaterial solicitud on detalle.RequisicionMaterialId = solicitud.RequisicionMaterialId
     INNER JOIN tblUnidadDeMedida AS um ON detalle.UnidadMedidaId = um.UnidadDeMedidaId
	 INNER JOIN tblDependencia AS unidadAdministrativa ON detalle.UnidadAdministrativaId = unidadAdministrativa.DependenciaId
	 INNER JOIN tblProyecto AS proyecto ON detalle.ProyectoId = proyecto.ProyectoId
	 INNER JOIN tblTipoGasto AS tipoGasto ON detalle.TipoGastoId = tipoGasto.TipoGastoId
	 INNER JOIN GRtblControlMaestro AS estatus ON detalle.EstatusId = estatus.ControlId
	 CROSS APPLY 
	 (
			SELECT SUM(Cantidad) AS Existencia
			FROM ARtblAlmacenProducto AS almacenProducto
				 INNER JOIN tblCuentaPresupuestalEgr AS cuentaPresupuestal ON almacenProducto.CuentaPresupuestalId = cuentaPresupuestal.CuentaPresupuestalEgrId
				 INNER JOIN ARtblControlMaestroConfiguracionArea area on area.AreaId = solicitud.AreaId and area.Borrado = 0 
				 INNER JOIN ARtblControlMaestroConfiguracionAreaAlmacen areaAlmacen on area.ConfiguracionAreaId = areaAlmacen.ConfiguracionAreaId and areaAlmacen.Borrado = 0
			WHERE almacenProducto.ProductoId = detalle.ProductoId
				  AND almacenProducto.AlmacenId = areaAlmacen.AlmacenId
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
      AND detalle.EstatusId IN (88, 84, 90, 83, 80) -- Revisión, Por surtir, Surtido parcial, Por Comprar, En almacén
