SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspActualizaEstatusOrdenCompra]
	@ordenCompraId INT
AS
-- ==============================================================
-- Author:		Javier Elías
-- Create date: 17/08/2021
-- Modified date: 
-- Description:	Procedimiento para actualizar el Estatus de una OC
-- ==============================================================

-- Estatus de OC
DECLARE @ESTATUS_ACTIVA NVARCHAR(1) = 'A'
DECLARE @ESTATUS_CANCELADA NVARCHAR(1) = 'C'
DECLARE @ESTATUS_RECIBO_PARCIAL NVARCHAR(1) = 'I'
DECLARE @ESTATUS_RECIBIDA NVARCHAR(1) = 'R'

-- Variables para contar los detalles de cada estatus
DECLARE @contador_Activo INT = 0
DECLARE @contador_Cancelado INT = 0
DECLARE @contador_Recibo_Parcial INT = 0
DECLARE @contador_Recibido INT = 0

-- Contamos los registros para cada estatus
SELECT @contador_Activo = SUM(CASE WHEN Status = @ESTATUS_ACTIVA THEN 1 ELSE 0 END),
       @contador_Cancelado = SUM(CASE WHEN Status = @ESTATUS_CANCELADA THEN 1 ELSE 0 END),
       @contador_Recibo_Parcial = SUM(CASE WHEN Status = @ESTATUS_RECIBO_PARCIAL THEN 1 ELSE 0 END),
       @contador_Recibido = SUM(CASE WHEN Status = @ESTATUS_RECIBIDA THEN 1 ELSE 0 END)
FROM tblOrdenCompraDet
WHERE OrdenCompraId = @ordenCompraId

-- Contar total de registros de la OC
DECLARE @contador_registros INT = ( SELECT COUNT(OrdenCompraDetId) FROM tblOrdenCompraDet WHERE OrdenCompraId = @ordenCompraId AND Status != @ESTATUS_CANCELADA )

-- Nuevo Estatus para la OC
DECLARE @estatusNuevoId NVARCHAR(1) = NULL

-- Si no existen detalles o todos tienen estatus Cancelado se cancelará la OC
IF ( @contador_registros = 0 )
BEGIN
		SET @estatusNuevoId = @ESTATUS_CANCELADA
END

-- Si todos los detalles no cancelados tienen estatus Recibido
ELSE IF ( @contador_registros = @contador_Recibido )
BEGIN
		SET @estatusNuevoId = @ESTATUS_RECIBIDA
END

-- Si existe por lo menos un detalle con estatus de Recibo Parcial
ELSE IF ( @contador_Recibo_Parcial > 0 )
BEGIN
		SET @estatusNuevoId = @ESTATUS_RECIBO_PARCIAL
END

-- Si no cumple con los anteriores dejamos la OC como Activa
ELSE
BEGIN
		SET @estatusNuevoId = @ESTATUS_ACTIVA
END

-- Actualizamos el Estatus de la OC
UPDATE tblOrdenCompra SET Status = ISNULL(@estatusNuevoId, Status) WHERE OrdenCompraId = @ordenCompraId