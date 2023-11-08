SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspConsultaDependenciasPorAreaId]
		@areaId VARCHAR (6)
AS
-- ========================================================
-- Author:		Javier Elías
-- Create date: 04/03/2022
-- Modified date: 
-- Description:	Procedure para obtener las Dependencias por Área
--						para una Solicitud de Materiales
-- ========================================================
SELECT DISTINCT dependencia.*
FROM ARtblControlMaestroConfiguracionArea AS configuracion
     INNER JOIN ARtblControlMaestroConfiguracionAreaProyecto AS unidadesProyectos ON configuracion.ConfiguracionAreaId = unidadesProyectos.ConfiguracionAreaId AND unidadesProyectos.Borrado = 0
	 INNER JOIN tblProyecto_Dependencia AS proyectoDependencia ON ProyectoDependenciaId = idPD
	 INNER JOIN tblDependencia AS dependencia ON proyectoDependencia.DependenciaId = dependencia.DependenciaId
WHERE configuracion.AreaId = @areaId
      AND configuracion.Borrado = 0
ORDER BY dependencia.DependenciaId
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspConsultaProyectosPorAreaId]
		@areaId VARCHAR (6)
AS
-- ========================================================
-- Author:		Javier Elías
-- Create date: 04/03/2022
-- Modified date: 
-- Description:	Procedure para obtener los Proyectos por Área
--						para una Solicitud de Materiales
-- ========================================================
SELECT DISTINCT proyecto.*
FROM ARtblControlMaestroConfiguracionArea AS configuracion
     INNER JOIN ARtblControlMaestroConfiguracionAreaProyecto AS unidadesProyectos ON configuracion.ConfiguracionAreaId = unidadesProyectos.ConfiguracionAreaId AND unidadesProyectos.Borrado = 0
	 INNER JOIN tblProyecto_Dependencia AS proyectoDependencia ON ProyectoDependenciaId = idPD
	 INNER JOIN tblProyecto AS proyecto ON proyectoDependencia.ProyectoId = proyecto.ProyectoId
WHERE configuracion.AreaId = @areaId
      AND configuracion.Borrado = 0
ORDER BY proyecto.ProyectoId
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ARspConsultaDependenciasProyectosPorAreaId]
		@areaId VARCHAR (6)
AS
-- ========================================================
-- Author:		Javier Elías
-- Create date: 04/03/2022
-- Modified date: 
-- Description:	Procedure para obtener las Dependencias / Proyectos
--						por Área para una Solicitud de Materiales
-- ========================================================
SELECT DISTINCT proyectoDependencia.*
FROM ARtblControlMaestroConfiguracionArea AS configuracion
     INNER JOIN ARtblControlMaestroConfiguracionAreaProyecto AS unidadesProyectos ON configuracion.ConfiguracionAreaId = unidadesProyectos.ConfiguracionAreaId AND unidadesProyectos.Borrado = 0
	 INNER JOIN tblProyecto_Dependencia AS proyectoDependencia ON ProyectoDependenciaId = idPD
WHERE configuracion.AreaId = @areaId
      AND configuracion.Borrado = 0
GO