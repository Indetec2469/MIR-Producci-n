SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspActualizaEstatusRequisicionMaterial]
	@requisicionId INT
AS
-- ==============================================================
-- Author:		Javier Elías
-- Create date: 24/06/2021
-- Modified date: 10/08/2021
-- Description:	Procedimiento para actualizar el Estatus de una Requisición
-- ==============================================================

-- Estatus de la Requisición
DECLARE @ESTATUS_AUTORIZADA INT = 64
DECLARE @ESTATUS_CANCELADA INT = 65
DECLARE @ESTATUS_CERRADA INT = 66
DECLARE @ESTATUS_EN_ALMACEN INT = 67
DECLARE @ESTATUS_EN_PROCESO INT = 68
DECLARE @ESTATUS_ENVIADA INT = 69
DECLARE @ESTATUS_FINALIZADA INT = 70
DECLARE @ESTATUS_GUARDADA INT = 71
DECLARE @ESTATUS_ORDEN_COMPRA INT = 72
DECLARE @ESTATUS_POR_COMPRAR INT = 73
DECLARE @ESTATUS_RECHAZADA INT = 74
DECLARE @ESTATUS_REQUISICION_COMPRA INT = 75
DECLARE @ESTATUS_REVISION INT = 76 

-- Estatus de los detalles de la Requisición
DECLARE @DETALLE_ACTIVO INT = 77
DECLARE @DETALLE_CANCELADO INT = 78
DECLARE @DETALLE_CERRADO INT = 79
DECLARE @DETALLE_EN_ALMACEN INT = 80
DECLARE @DETALLE_ENVIADO INT = 81
DECLARE @DETALLE_MODIFICADO INT = 82
DECLARE @DETALLE_POR_COMPRAR INT = 83
DECLARE @DETALLE_POR_SURTIR INT = 84
DECLARE @DETALLE_RECHAZADO INT = 85
DECLARE @DETALLE_RELACIONADO_OC INT = 86
DECLARE @DETALLE_RELACIONADO_RC INT = 87
DECLARE @DETALLE_REVISION INT = 88
DECLARE @DETALLE_SURTIDO INT = 89
DECLARE @DETALLE_SURTIDO_PARCIAL INT = 90

-- Variables para contar los detalles de cada estatus
DECLARE @contador_Activo INT = 0
DECLARE @contador_Cancelado INT = 0
DECLARE @contador_Cerrado INT = 0
DECLARE @contador_En_almacen INT = 0
DECLARE @contador_Enviado INT = 0
DECLARE @contador_Modificado INT = 0
DECLARE @contador_Por_comprar INT = 0
DECLARE @contador_Por_surtir INT = 0
DECLARE @contador_Rechazado INT = 0
DECLARE @contador_Relacionado_OC INT = 0
DECLARE @contador_Relacionado_RC INT = 0
DECLARE @contador_Revision INT = 0
DECLARE @contador_Surtido INT = 0
DECLARE @contador_Surtido_Parcial INT = 0

-- Contamos los registros para cada estatus
SELECT @contador_Activo = SUM(CASE WHEN EstatusId = @DETALLE_ACTIVO THEN 1 ELSE 0 END),
       @contador_Cancelado = SUM(CASE WHEN EstatusId = @DETALLE_CANCELADO THEN 1 ELSE 0 END),
       @contador_Cerrado = SUM(CASE WHEN EstatusId = @DETALLE_CERRADO THEN 1 ELSE 0 END),
       @contador_En_almacen = SUM(CASE WHEN EstatusId = @DETALLE_EN_ALMACEN THEN 1 ELSE 0 END),
       @contador_Enviado = SUM(CASE WHEN EstatusId = @DETALLE_ENVIADO THEN 1 ELSE 0 END),
       @contador_Modificado = SUM(CASE WHEN EstatusId = @DETALLE_MODIFICADO THEN 1 ELSE 0 END),
       @contador_Por_comprar = SUM(CASE WHEN EstatusId = @DETALLE_POR_COMPRAR THEN 1 ELSE 0 END),
       @contador_Por_surtir = SUM(CASE WHEN EstatusId = @DETALLE_POR_SURTIR THEN 1 ELSE 0 END),
       @contador_Rechazado = SUM(CASE WHEN EstatusId = @DETALLE_RECHAZADO THEN 1 ELSE 0 END),
       @contador_Relacionado_OC = SUM(CASE WHEN EstatusId = @DETALLE_RELACIONADO_OC THEN 1 ELSE 0 END),
       @contador_Relacionado_RC = SUM(CASE WHEN EstatusId = @DETALLE_RELACIONADO_RC THEN 1 ELSE 0 END),
       @contador_Revision = SUM(CASE WHEN EstatusId = @DETALLE_REVISION THEN 1 ELSE 0 END),
       @contador_Surtido = SUM(CASE WHEN EstatusId = @DETALLE_SURTIDO THEN 1 ELSE 0 END),
       @contador_Surtido_Parcial = SUM(CASE WHEN EstatusId = @DETALLE_SURTIDO_PARCIAL THEN 1 ELSE 0 END)
FROM ARtblRequisicionMaterialDetalle
WHERE RequisicionMaterialId = @requisicionId

-- Contar total de registros de la Requisición
DECLARE @contador_registros INT = ( SELECT COUNT(RequisicionMaterialDetalleId) FROM ARtblRequisicionMaterialDetalle WHERE RequisicionMaterialId = @requisicionId AND EstatusId != @DETALLE_CANCELADO )

-- Nuevo Estatus para la Requisición
DECLARE @estatusNuevoId INT = NULL

-- Si no existen detalles o todos tienen estatus Cancelado se cancelará la Requisición
IF ( @contador_registros = 0 )
BEGIN
		SET @estatusNuevoId = @ESTATUS_CANCELADA
END

-- Si todos los detalles no cancelados tienen estatus Rechazado se rechazará la Requisición
ELSE IF ( @contador_registros = @contador_Rechazado )
BEGIN
		SET @estatusNuevoId = @ESTATUS_RECHAZADA
END

ELSE 
BEGIN
		-- Quitamos los detalles con estatus Rechazado al contador de detalles no cancelados
		SET @contador_registros = @contador_registros - @contador_Rechazado

		-- Si todos los detalles no cancelados tienen estatus Surtido la Requisición pasa a Finalizada
		IF ( @contador_registros = @contador_Surtido )
		BEGIN
				SET @estatusNuevoId = @ESTATUS_FINALIZADA
		END

		-- Si todos los detalles no cancelados tienen estatus en Revisión la Requisición pasa a Revisión
		ELSE IF ( @contador_registros = @contador_Revision )
		BEGIN
				SET @estatusNuevoId = @ESTATUS_REVISION
		END

		-- Si todos los detalles no cancelados tienen estatus Relacionado a OC la Requisición pasa a Orden de Compra
		ELSE IF ( @contador_registros = @contador_Relacionado_OC )
		BEGIN
				SET @estatusNuevoId = @ESTATUS_ORDEN_COMPRA
		END

		-- Si todos los detalles no cancelados tienen estatus Relacionado a RC la Requisición pasa a Invitación de Compra
		ELSE IF ( @contador_registros = @contador_Relacionado_RC )
		BEGIN
				SET @estatusNuevoId = @ESTATUS_REQUISICION_COMPRA
		END

		-- Si existe por lo menos un detalle con estatus Por Surtir, Surtido o con Surtido Parcial
		-- la Requisición pasa a En Proceso
		ELSE IF ( @contador_Por_Surtir > 0 OR @contador_Surtido > 0 OR @contador_Surtido_Parcial > 0 )
		BEGIN
				SET @estatusNuevoId = @ESTATUS_EN_PROCESO
		END

		-- Si existe por lo menos un detalle con estatus Por Comprar y no hay detalles con estatus Por Surtir, Surtido o con Surtido Parcial
		-- la requisición pasa a Por Comprar
		ELSE IF ( @contador_Por_comprar > 0 )
		BEGIN
				SET @estatusNuevoId = @ESTATUS_POR_COMPRAR
		END
END

-- Actualizamos el Estatus de la Requisición
UPDATE ARtblRequisicionMaterial SET EstatusId = ISNULL(@estatusNuevoId, EstatusId) WHERE RequisicionMaterialId = @requisicionId