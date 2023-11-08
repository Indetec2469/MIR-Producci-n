SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ================================================
-- Author:		Alonso Soto
-- Create date: 11/01/2022
-- Modified date: 22/02/2022
-- Description:	SP para obtener los Empleados a los que 
--						se notificará de Alertas
-- ================================================
CREATE OR ALTER PROCEDURE [dbo].[GRspGetEmpleadosANotificarAlertas] 
	@alertaDefinicionId INT,
	@referenciaProcesoId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @tblEmpleadosParaAlertas TABLE 
	(
		empleadoId INT,
		figuraId INT,
		tipoNotificacionId INT,
		medioCorreoElectronico BIT,
		medioPlataforma BIT
	)

	--DECLARE @solicitudId INT
	DECLARE @empleadoSolicitanteId INT
	DECLARE @areaAdscripcionId VARCHAR(6)
	DECLARE @empleadoAutorizacionInmediatoId INT
	DECLARE @notificarAJefeInmediato BIT 
	DECLARE @notificarASolicitante BIT
	
	DECLARE @ALERTA_CONFIGURACION_FIGURA_JEFE_INMEDIATO_ID INT = 124
	DECLARE @ALERTA_CONFIGURACION_FIGURA_SOLICITANTE_ID INT = 125

	DECLARE @nombreTabla VARCHAR(100)
	DECLARE @nombreCampoId VARCHAR(50)
	DECLARE @nombreCampoUsuarioRegistro VARCHAR(100)
	DECLARE @updateQuery VARCHAR(4000)

	SELECT @nombreTabla = TablaReferencia,
		   @nombreCampoId = CampoId,
		   @nombreCampoUsuarioRegistro = CampoUsuarioRegistro
	FROM GRtblAlertaDefinicion
	WHERE AlertaDefinicionId = @alertaDefinicionId
				
	-- Buscar los datos del trámite
	SET @updateQuery = 
	'SELECT empleado.EmpleadoId,
			empleado.AreaAdscripcionId,
			NULL /*CASE WHEN NodoPadreId IS NULL THEN empleado.EmpleadoId ELSE dbo.GRfnGetAutorizacionUsuarioInmediatoId(empleado.AreaAdscripcionId) END*/
	FROM ' + @nombreTabla + ' AS tabla
		INNER JOIN GRtblUsuario AS usuario ON tabla.' + @nombreCampoUsuarioRegistro + ' = usuario.UsuarioId
		INNER JOIN RHtblEmpleado AS empleado ON usuario.EmpleadoId = empleado.EmpleadoId
	WHERE ' + @nombreCampoId + ' = ' + CONVERT(VARCHAR(10), @referenciaProcesoId)

	DECLARE @tblTemp TABLE (
		empleadoSolicitanteId INT,
		areaAdscripcionId VARCHAR(6),
		empleadoAutorizacionInmediatoId INT
	)

	INSERT INTO @tblTemp
	EXEC(@updateQuery)

	SELECT @empleadoSolicitanteId = empleadoSolicitanteId,
			@areaAdscripcionId = areaAdscripcionId,
			@empleadoAutorizacionInmediatoId = empleadoAutorizacionInmediatoId
	FROM @tblTemp
	
	-- Obtenemos los empleados configurados para recibir alerta, asi como el tipo de notificacion y el medio por el que se les notificara
	INSERT INTO @tblEmpleadosParaAlertas
	SELECT configuracion.empleadoId, 
		   configuracion.FiguraId, 
		   configuracion.TipoNotificacionId, 
		   configuracion.EnCorreoElectronico, 
		   configuracion.EnPlataforma 
	FROM GRtblAlertaDefinicion AS definicion
		INNER JOIN GRtblAlertaEtapaAccion AS etapaAccion ON definicion.AlertaEtapaAccionId = etapaAccion.AlertaEtapaAccionId
		INNER JOIN GRtblAlertaConfiguracion AS configuracion ON etapaAccion.AlertaEtapaAccionId = configuracion.AlertaEtapaAccionId 
	WHERE definicion.AlertaDefinicionId = @alertaDefinicionId
	      AND configuracion.EstatusId = 1	--EstatusRegistro Activo
		  AND definicion.Borrado = 0 

	-- Verificamos si dentro de los usuarios a los que se les tiene que notificar se encuentra la figura Jefe Inmediato o Solicitante para obtener los 
	-- Id de empleados que corresponden
	SELECT @notificarAJefeInmediato =  (CASE WHEN COUNT(figuraId) > 0 THEN 1 ELSE 0 END)
	FROM @tblEmpleadosParaAlertas
	WHERE figuraId = @ALERTA_CONFIGURACION_FIGURA_JEFE_INMEDIATO_ID
	
	SELECT @notificarASolicitante =  (CASE WHEN COUNT(figuraId) > 0 THEN 1 ELSE 0 END)
	FROM @tblEmpleadosParaAlertas
	WHERE figuraId =  @ALERTA_CONFIGURACION_FIGURA_SOLICITANTE_ID

	-- Si hay que notificar al jefe inmediato, actualizamos el id de la figura Jefe Inmediato por el id del empleado que le corresponde en la tabla temporal
	IF(@notificarAJefeInmediato = 1)
	BEGIN

		UPDATE @tblEmpleadosParaAlertas
		SET EmpleadoId = @empleadoAutorizacionInmediatoId
		WHERE FiguraId = @ALERTA_CONFIGURACION_FIGURA_JEFE_INMEDIATO_ID		

	END

	-- Si hay que notificar al solicitante, actualizamos el id de la figura Solicitante por el id del empleado que le corresponde en la tabla temporal
	IF(@notificarASolicitante = 1)
	BEGIN

		UPDATE @tblEmpleadosParaAlertas
		SET EmpleadoId = @empleadoSolicitanteId
		WHERE FiguraId = @ALERTA_CONFIGURACION_FIGURA_SOLICITANTE_ID

	END

	SELECT * FROM @tblEmpleadosParaAlertas
END
GO