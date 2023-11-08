SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER   PROCEDURE [dbo].[GRspAlertasConfiguracionEmpleados]
AS
-- =================================================
-- Author:		Javier Elías
-- Create date: 04/02/2022
-- Modified date: 27/09/2022
-- Description:	Procedimiento para obtener los Empleados
--						para la Ficha de Configuración de Alertas.
-- =================================================
	SELECT *
	FROM
	(
		SELECT ControlId AS Id,
			   Valor AS Nombre,
			   CONVERT(BIT, 1) AS Figura
		FROM GRtblControlMaestro
		WHERE Control = 'AlertaConfiguracionFigura'
			  AND Activo = 1
			  AND ControlId = 125 -- Solicitante
    
		UNION ALL
    
		SELECT EmpleadoId AS Id,
			   dbo.RHfnGetNombreCompletoEmpleado(EmpleadoId) AS Nombre,
			   CONVERT(BIT, 0) AS Figura
		FROM RHtblEmpleado
		WHERE Vigente = 1
			  AND EstatusId = 1 -- Activo
	) AS Usuarios
	ORDER BY Figura DESC,
			 Nombre
