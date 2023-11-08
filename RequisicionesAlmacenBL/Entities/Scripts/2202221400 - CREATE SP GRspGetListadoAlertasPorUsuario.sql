SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ==========================================================
-- Author:		Alonso Soto
-- Create date: <Create Date,,>
-- Modified: Javier Elías
-- Create date: 21/02/2021
-- Description:	Funcion para obtener el listado de Alertas por usuario
-- ==========================================================
CREATE OR ALTER PROCEDURE [dbo].[GRspGetListadoAlertasPorUsuario] ( @usuarioId INT )
AS

	DECLARE @tblAlertas TABLE 
	(
		AlertaId INT, 
		AlertaIniciadaPor NVARCHAR(1000),
		Fecha NVARCHAR(100),
		TipoMovimiento NVARCHAR(500),
		Tramite NVARCHAR(2550),
		Estatus NVARCHAR(500),
		RutaAccion NVARCHAR(4000),
		TipoAlertaId INT,
		EtapaAccionAlAutorizarId INT,
		EtapaAccionAlRevisionId INT,
		EtapaAccionAlRechazarId INT,
		MotivoRechazo VARCHAR (2000)
	)

	INSERT INTO @tblAlertas
	SELECT alerta.AlertaId AS AlertaId,  
		   dbo.RHfnGetNombreCompletoEmpleado(usuarioCreadoPor.EmpleadoId) AS AlertaIniciadaPor,
		   dbo.GRfnGetFechaConFormato(alerta.FechaCreacion, 1) AS Fecha,
		   definicion.NombreCorto AS TipoMovimiento,
		   alerta.TextoRepresentativo AS Tramite,
		   estatus.Valor AS Estatus,
		   REPLACE(definicion.RutaAccion, '{id}', alerta.ReferenciaProcesoId) AS RutaAccion,
		   122 AS tipoAlertaId, --Tipo Autorizacion
		   EtapaAccionAlAutorizarId,
		   EtapaAccionAlRevisionId,
		   EtapaAccionAlRechazarId,
		   MotivoRechazo
	FROM GRtblAlerta AS alerta
		INNER JOIN GRtblAlertaDefinicion AS definicion ON alerta.AlertaDefinicionId = definicion.AlertaDefinicionId
		INNER JOIN GRtblControlMaestro AS estatus ON alerta.EstatusId = estatus.ControlId
		INNER JOIN GRtblUsuario AS usuarioCreadoPor ON alerta.CreadoPorId = usuarioCreadoPor.UsuarioId
		INNER JOIN GRtblAlertaAutorizacion AS detalle ON detalle.AlertaId = alerta.AlertaId
		INNER JOIN GRtblUsuario AS usuario ON detalle.EmpleadoId = usuario.EmpleadoId AND usuario.UsuarioId = @usuarioId
	WHERE alerta.EstatusId = 118 --En proceso
		AND detalle.Vista = 0
	ORDER BY alerta.FechaCreacion ASC
	
	INSERT INTO  @tblAlertas 
	SELECT alerta.AlertaId AS AlertaId,  
		   dbo.RHfnGetNombreCompletoEmpleado(usuarioCreadoPor.EmpleadoId) AS AlertaIniciadaPor,
		   dbo.GRfnGetFechaConFormato(alerta.FechaCreacion, 1) AS Fecha,
		   definicion.NombreCorto AS TipoMovimiento,
		   alerta.TextoRepresentativo AS Tramite,
		   estatus.Valor AS Estatus,
		   REPLACE(definicion.RutaAccion, '{id}', alerta.ReferenciaProcesoId) AS RutaAccion,
		   123 AS tipoAlertaId, --Tipo Notificacion
		   NULL AS EtapaAccionAlAutorizarId,
		   NULL AS EtapaAccionAlRevisionId,
		   NULL AS EtapaAccionAlRechazarId,
		   NULL AS MotivoRechazo
	FROM GRtblAlerta AS alerta
		INNER JOIN GRtblAlertaDefinicion AS definicion ON alerta.AlertaDefinicionId = definicion.AlertaDefinicionId
		INNER JOIN GRtblControlMaestro AS estatus ON alerta.EstatusId = estatus.ControlId
		INNER JOIN GRtblUsuario AS usuarioCreadoPor ON alerta.CreadoPorId = usuarioCreadoPor.UsuarioId
		INNER JOIN GRtblAlertaNotificacion AS detalle ON detalle.AlertaId = alerta.AlertaId
		INNER JOIN GRtblUsuario AS usuario ON detalle.EmpleadoId = usuario.EmpleadoId AND usuario.UsuarioId = @usuarioId
	WHERE alerta.EstatusId != 121 --Finalizada
		AND detalle.Vista = 0
	ORDER BY alerta.FechaCreacion DESC

	SELECT * FROM @tblAlertas

GO