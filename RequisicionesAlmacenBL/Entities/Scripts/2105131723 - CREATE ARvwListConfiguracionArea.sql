SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER VIEW [dbo].[ARvwListConfiguracionArea]
AS
-- ============================================================
-- Author:		Javier Elías
-- Create date: 12/05/2021
-- Modified date: 
-- Description:	View para obtener el Listado de Configuración de Áreas
-- ============================================================
SELECT configuracion.*,
       area.DependenciaId AS Codigo,
	   area.Nombre AS Area,
	   ISNULL(detalle.Proyectos, 0) AS Proyectos,
	   ISNULL(detalle.UnidadesAdministrativas, 0) AS UnidadesAdministrativas,
	   0 AS EstatusId
FROM ARtblControlMaestroConfiguracionArea AS configuracion
     INNER JOIN tblDependencia AS area ON configuracion.AreaId = area.DependenciaId
	 OUTER APPLY 
	 (
			SELECT ConfiguracionAreaId,
				   COUNT(DISTINCT proyectoDependencia.ProyectoId) AS Proyectos,
				   COUNT(DISTINCT proyectoDependencia.DependenciaId) AS UnidadesAdministrativas
			FROM ARtblControlMaestroConfiguracionAreaProyecto AS detalle
				 INNER JOIN tblProyecto_Dependencia AS proyectoDependencia ON detalle.ProyectoDependenciaId = proyectoDependencia.idPD
			WHERE detalle.ConfiguracionAreaId = configuracion.ConfiguracionAreaId
				  AND detalle.Borrado = 0
			GROUP BY ConfiguracionAreaId
	 ) AS detalle
WHERE configuracion.Borrado = 0
GO