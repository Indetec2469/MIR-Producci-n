SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER TRIGGER [dbo].[ARtrgProcesaAlertaRequisicion] ON [dbo].[ARtblRequisicionMaterial] AFTER UPDATE
AS
BEGIN		
		
		DECLARE @ESTATUS_REQUISICION_AUTORIZADA INT = 64
		DECLARE @ESTATUS_REQUISICION_REVISION INT = 76
		DECLARE @ESTATUS_REQUISICION_RECHAZADA INT = 74

		DECLARE @ESTATUS_DETALLE_ENVIADO INT = 81
		DECLARE @ESTATUS_DETALLE_REVISION INT = 88
		DECLARE @ESTATUS_DETALLE_RECHAZADO INT = 85
		DECLARE @ESTATUS_DETALLE_POR_COMPRAR INT = 83
		DECLARE @ESTATUS_DETALLE_POR_SURTIR INT = 84

		DECLARE @nuevoEstatusId INT = ( SELECT EstatusId FROM Inserted )

		IF ( @nuevoEstatusId = @ESTATUS_REQUISICION_AUTORIZADA )
		BEGIN		
			UPDATE detalles
			  SET
				  EstatusId = CASE WHEN ISNULL(Existencia, 0) > 0
							  THEN @ESTATUS_DETALLE_POR_SURTIR
							  ELSE @ESTATUS_DETALLE_POR_COMPRAR
							  END
			FROM inserted
				 INNER JOIN ARtblRequisicionMaterialDetalle AS detalles ON inserted.RequisicionMaterialId = detalles.RequisicionMaterialId AND detalles.EstatusId = @ESTATUS_DETALLE_ENVIADO
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
		END

		ELSE IF ( @nuevoEstatusId IN (@ESTATUS_REQUISICION_REVISION, @ESTATUS_REQUISICION_RECHAZADA) )
		BEGIN		
			UPDATE detalles
			  SET
				  EstatusId = CASE WHEN @nuevoEstatusId = @ESTATUS_REQUISICION_REVISION
							  THEN @ESTATUS_DETALLE_REVISION
							  ELSE @ESTATUS_DETALLE_RECHAZADO
							  END
			FROM inserted
				 INNER JOIN ARtblRequisicionMaterialDetalle AS detalles ON inserted.RequisicionMaterialId = detalles.RequisicionMaterialId AND detalles.EstatusId = @ESTATUS_DETALLE_ENVIADO
		END
END