SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspConsultaRequisicionPorSurtirExistencias]
	@requisicionId INT
AS
-- =======================================================================
-- Author:		Javier Elías
-- Create date: 23/06/2021
-- Modified date: 05/08/2022
-- Description:	Procedimiento para Obtener la existenica de Almacén / Producto
--						para una Requisición Pendiente por Surtir.
-- =======================================================================
SELECT DISTINCT
       almacenProducto.AlmacenProductoId,
	   fuenteFinanciamiento.RamoId AS FuenteFinanciamientoId,
	   fuenteFinanciamiento.Nombre AS FuenteFinanciamiento,
	   detalle.AlmacenId, 
	   detalle.ProductoId, 
	   detalle.UnidadAdministrativaId, 
	   detalle.ProyectoId, 
	   detalle.TipoGastoId,
       almacenProducto.Cantidad AS Existencia,
	   CONVERT(DECIMAL(28, 10), NULL) AS CantidadSurtir
FROM ARtblRequisicionMaterialDetalle AS detalle
     INNER JOIN ARtblAlmacenProducto AS almacenProducto ON detalle.AlmacenId = almacenProducto.AlmacenId
                                                                AND detalle.ProductoId = almacenProducto.ProductoId
																AND almacenProducto.Borrado = 0
																AND almacenProducto.Cantidad > 0
	 INNER JOIN tblCuentaPresupuestalEgr AS cuentaPresupuestal ON almacenProducto.CuentaPresupuestalId = cuentaPresupuestal.CuentaPresupuestalEgrId
                                                                       AND detalle.UnidadAdministrativaId = cuentaPresupuestal.DependenciaId
																	   AND detalle.ProyectoId = cuentaPresupuestal.ProyectoId
																	   AND detalle.TipoGastoId = cuentaPresupuestal.TipoGastoId
     INNER JOIN tblRamo AS fuenteFinanciamiento ON cuentaPresupuestal.RamoId = fuenteFinanciamiento.RamoId
WHERE detalle.RequisicionMaterialId = @requisicionId
      AND detalle.EstatusId != 78 -- Cancelado