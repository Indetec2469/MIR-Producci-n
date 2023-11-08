SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspActualizaEstatusInvitacionCompra]
	@invitacionId INT
AS
-- =====================================================================
-- Author:		Javier Elías
-- Create date: 22/11/2021
-- Modified date: 
-- Description:	Procedimiento para actualizar el Estatus de una Invitación de Compra
-- =====================================================================

-- Estatus de la Invitación Compra
DECLARE @INVITACION_COMPRA_CANCELADA INT = 91
DECLARE @INVITACION_COMPRA_CONVERTIDA_PARCIALMENTE INT = 105
DECLARE @INVITACION_COMPRA_FINALIZADA INT = 106
DECLARE @INVITACION_COMPRA_GUARDADA INT = 92

-- Estatus de los detalles de la Invitación Compra
DECLARE @INVITACION_COMPRA_DETALLE_ACTIVO INT = 93
DECLARE @INVITACION_COMPRA_DETALLE_CANCELADO INT = 94
DECLARE @INVITACION_COMPRA_DETALLE_CONVERTIDO_OC INT = 104

-- Estatus de los detalles de la Requisición
DECLARE @REQUISICION_DETALLE_RELACIONADO_OC INT = 86

-- Variables para contar los detalles de cada estatus
DECLARE @contador_Activo INT = 0
DECLARE @contador_Cancelado INT = 0
DECLARE @contador_Convertido_OC INT = 0

-- Contamos los registros para cada estatus
SELECT @contador_Activo = SUM(CASE WHEN EstatusId = @INVITACION_COMPRA_DETALLE_ACTIVO THEN 1 ELSE 0 END),
       @contador_Cancelado = SUM(CASE WHEN EstatusId = @INVITACION_COMPRA_DETALLE_CANCELADO THEN 1 ELSE 0 END),
       @contador_Convertido_OC = SUM(CASE WHEN EstatusId = @INVITACION_COMPRA_DETALLE_CONVERTIDO_OC THEN 1 ELSE 0 END)
FROM ARtblInvitacionCompraDetalle
WHERE InvitacionCompraId = @invitacionId

-- Contar total de registros de la Invitación
DECLARE @contador_registros INT = ( SELECT COUNT(InvitacionCompraDetalleId) FROM ARtblInvitacionCompraDetalle WHERE InvitacionCompraId = @invitacionId AND EstatusId != @INVITACION_COMPRA_DETALLE_CANCELADO )

-- Nuevo Estatus para la Invitación
DECLARE @estatusNuevoId INT = NULL

-- Si no existen detalles o todos tienen estatus Cancelado se cancelará la Invitación
IF ( @contador_registros = 0 )
BEGIN
		SET @estatusNuevoId = @INVITACION_COMPRA_CANCELADA
END

ELSE 
BEGIN
		-- Si todos los detalles no cancelados tienen estatus Convertido la Invitación pasa a Finalizada
		IF ( @contador_registros = @contador_Convertido_OC )
		BEGIN
				SET @estatusNuevoId = @INVITACION_COMPRA_FINALIZADA
		END

		-- Si existe por lo menos un detalle con estatus Convertido a OC la Invitación pasa a Convertida Parcialmente
		ELSE IF ( @contador_Convertido_OC > 0 )
		BEGIN
				SET @estatusNuevoId = @INVITACION_COMPRA_CONVERTIDA_PARCIALMENTE
		END
END

-- Actualizamos el Estatus de la Invitación
UPDATE ARtblInvitacionCompra SET EstatusId = ISNULL(@estatusNuevoId, EstatusId) WHERE InvitacionCompraId = @invitacionId

-- Actualizamos los detalles de la Requisición que se convirtió
UPDATE requisicionDetalle
  SET
      requisicionDetalle.EstatusId = @REQUISICION_DETALLE_RELACIONADO_OC
FROM ARtblInvitacionCompraDetalle AS invitacionCompraDetalle
     INNER JOIN ARtblInvitacionArticuloDetalle AS invitacionArticuloDetalle ON invitacionCompraDetalle.InvitacionArticuloDetalleId = invitacionArticuloDetalle.InvitacionArticuloDetalleId
     INNER JOIN ARtblRequisicionMaterialDetalle AS requisicionDetalle ON invitacionArticuloDetalle.RequisicionMaterialDetalleId = requisicionDetalle.RequisicionMaterialDetalleId
WHERE invitacionCompraDetalle.InvitacionCompraId = @invitacionId
      AND invitacionCompraDetalle.EstatusId = @INVITACION_COMPRA_DETALLE_CONVERTIDO_OC

-- Actualizamos el estatus de cada Requisición
DECLARE @tblRequisicionMaterial TABLE ( Id INT, Contador INT )
INSERT INTO @tblRequisicionMaterial
SELECT RequisicionMaterialId, ROW_NUMBER() OVER ( ORDER BY RequisicionMaterialId )
FROM 
(
SELECT DISTINCT requisicionDetalle.RequisicionMaterialId
FROM ARtblInvitacionCompraDetalle AS invitacionCompraDetalle
     INNER JOIN ARtblInvitacionArticuloDetalle AS invitacionArticuloDetalle ON invitacionCompraDetalle.InvitacionArticuloDetalleId = invitacionArticuloDetalle.InvitacionArticuloDetalleId
     INNER JOIN ARtblRequisicionMaterialDetalle AS requisicionDetalle ON invitacionArticuloDetalle.RequisicionMaterialDetalleId = requisicionDetalle.RequisicionMaterialDetalleId
WHERE invitacionCompraDetalle.InvitacionCompraId = @invitacionId
      AND invitacionCompraDetalle.EstatusId = @INVITACION_COMPRA_DETALLE_CONVERTIDO_OC
) AS todo

DECLARE @contador INT = 1
DECLARE @contadorRequisiciones INT = ( SELECT MAX(Contador) FROM @tblRequisicionMaterial )

WHILE ( @contador <= @contadorRequisiciones )
BEGIN
	DECLARE @requisicionId INT = ( SELECT Id FROM @tblRequisicionMaterial WHERE Contador = @contador )

	EXEC ARspActualizaEstatusRequisicionMaterial @requisicionId

	SET @contador = @contador + 1
END