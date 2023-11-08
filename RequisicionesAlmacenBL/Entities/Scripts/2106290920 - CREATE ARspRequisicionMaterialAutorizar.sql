SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspRequisicionMaterialAutorizar]
	@requisicionId INT
AS
/* ****************************************************************
 * ARspRequisicionMaterialAutorizar
 * ****************************************************************
 * Descripción: Procedimiento para Autorizar una Requisiciónde Material
 *
 * autor: 	Javier Elías
 * Fecha: 	25.06.2021
 * Versión: 1.0.0
 *****************************************************************
 * PARAMETROS DE ENTRADA: RequisicionId
 * PARAMETROS DE SALIDA:
 *****************************************************************
*/ 
UPDATE requisicion SET EstatusId = 61 --Autorizada
FROM ARtblRequisicionMaterial AS requisicion
WHERE requisicion.RequisicionMaterialId = @requisicionId

UPDATE detalles
  SET
      EstatusId = CASE WHEN ISNULL(Existencia, 0) >= detalles.Cantidad THEN 26 -- Pendiente por surtir
                  ELSE 62 -- Por Comprar
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
      AND EstatusId != 31 --Borrada