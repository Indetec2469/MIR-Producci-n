SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspRequisicionMaterialAutorizar]
	@requisicionId INT
AS
-- ============================================================
-- Author:		Javier Elías
-- Create date: 25.06.2021
-- Modified date: 06/08/2021
-- Description:	Procedimiento para Autorizar una Requisiciónde Material
-- ============================================================
UPDATE requisicion SET EstatusId = 64 --Autorizada
FROM ARtblRequisicionMaterial AS requisicion
WHERE requisicion.RequisicionMaterialId = @requisicionId

UPDATE detalles
  SET
      EstatusId = CASE WHEN ISNULL(Existencia, 0) > 0 THEN 84 -- Por surtir
                  ELSE 83 -- Por Comprar
                  END
FROM ARtblRequisicionMaterialDetalle AS detalles
     OUTER APPLY
	 (
		SELECT SUM(almacenProducto.Cantidad) AS Existencia
		FROM ARtblAlmacenProducto AS almacenProducto
			 INNER JOIN tblCuentaPresupuestalEgr AS cuentaPresupuestal ON almacenProducto.CuentaPresupuestalId = cuentaPresupuestal.CuentaPresupuestalEgrId
																		  AND detalles.UnidadAdministrativaId = cuentaPresupuestal.DependenciaId
																		  AND detalles.ProyectoId = cuentaPresupuestal.ProyectoId
																		  AND detalles.TipoGastoId = cuentaPresupuestal.TipoGastoId
		WHERE almacenProducto.AlmacenId = detalles.AlmacenId
			  AND almacenProducto.ProductoId = detalles.ProductoId
			  AND almacenProducto.Borrado = 0
	 ) AS existencia
WHERE detalles.RequisicionMaterialId = @requisicionId
      AND detalles.EstatusId != 78 -- Cancelado
GO