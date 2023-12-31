SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER   PROCEDURE [dbo].[ARspConsultaRequisicionPorSurtirExistencias]
	@requisicionId INT
AS
-- =======================================================================
-- Author:		Javier Elías
-- Create date: 23/06/2021
-- Modified date: 18/01/2023
-- Description:	Procedimiento para Obtener la existenica de Almacén / Producto
--						para una Requisición Pendiente por Surtir.
-- =======================================================================
SELECT DISTINCT
       almacenProducto.AlmacenProductoId,
	   fuenteFinanciamiento.RamoId AS FuenteFinanciamientoId,
	   fuenteFinanciamiento.Nombre AS FuenteFinanciamiento,
	   almacen.Nombre AS Almacen,
	   almacenProducto.AlmacenId, 
	   detalle.ProductoId, 
	   detalle.UnidadAdministrativaId, 
	   detalle.ProyectoId, 
	   detalle.TipoGastoId,
       almacenProducto.Cantidad AS Existencia,
	   CONVERT(DECIMAL(28, 10), NULL) AS CantidadSurtir
FROM ARtblRequisicionMaterialDetalle AS detalle
     INNER JOIN ARtblRequisicionMaterial solicitud on detalle.RequisicionMaterialId = solicitud.RequisicionMaterialId
	 INNER JOIN ARtblControlMaestroConfiguracionArea area on area.AreaId = solicitud.AreaId and area.Borrado = 0 
	 INNER JOIN ARtblControlMaestroConfiguracionAreaAlmacen areaAlmacen on area.ConfiguracionAreaId = areaAlmacen.ConfiguracionAreaId and areaAlmacen.Borrado = 0
     INNER JOIN ARtblAlmacenProducto AS almacenProducto ON detalle.ProductoId = almacenProducto.ProductoId
															    AND almacenProducto.AlmacenId = areaAlmacen.AlmacenId
																AND almacenProducto.Borrado = 0
																AND almacenProducto.Cantidad > 0
	 INNER JOIN tblCuentaPresupuestalEgr AS cuentaPresupuestal ON almacenProducto.CuentaPresupuestalId = cuentaPresupuestal.CuentaPresupuestalEgrId
                                                                       AND detalle.UnidadAdministrativaId = cuentaPresupuestal.DependenciaId
																	   AND detalle.ProyectoId = cuentaPresupuestal.ProyectoId
																	   AND detalle.TipoGastoId = cuentaPresupuestal.TipoGastoId
     INNER JOIN tblRamo AS fuenteFinanciamiento ON cuentaPresupuestal.RamoId = fuenteFinanciamiento.RamoId
	 INNER JOIN tblAlmacen AS almacen ON almacenProducto.AlmacenId = almacen.AlmacenId
WHERE detalle.RequisicionMaterialId = @requisicionId
      AND detalle.EstatusId != 78 -- Cancelado