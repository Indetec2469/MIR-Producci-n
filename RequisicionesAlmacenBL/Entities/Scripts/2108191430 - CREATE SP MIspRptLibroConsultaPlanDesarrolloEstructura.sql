SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/* ****************************************************************
 * MIspRptLibroConsultaPlanDesarrolloEstructura
 * ****************************************************************
 * Descripción: Consulta para obtener los datos de Plan Desarrollo Estructura para el reporte.
 * Autor: Rene Carrillo
 * Fecha: 19.08.2021
 * Versión: 1.0.0
 *****************************************************************
 * PARAMETROS DE ENTRADA: PlanDesarrolloId
 * PARAMETROS DE SALIDA: *
 *****************************************************************
*/

CREATE   PROCEDURE [dbo].[MIspRptLibroConsultaPlanDesarrolloEstructura]
	@PlanDesarrolloId INT
AS
	SELECT CONCAT('Nivel', ' ', ROW_NUMBER() OVER(ORDER BY pde.PlanDesarrolloId ASC), ': ', pde.Nombre) AS NombreNivel, pde.*
	FROM MItblPlanDesarrolloEstructura pde
	WHERE pde.PlanDesarrolloId = @PlanDesarrolloId
	ORDER BY pde.Orden
GO


