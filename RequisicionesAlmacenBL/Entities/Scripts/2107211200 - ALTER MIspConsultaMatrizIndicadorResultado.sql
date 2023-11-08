SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER   PROCEDURE [dbo].[MIspConsultaMatrizIndicadorResultado]
	@mirId INT
AS
/* ****************************************************************
 * spConsultaMatrizIndicadorResultado
 * ****************************************************************
 * Descripción: Buscamos el objeto de Matriz Indicador Resultado por el ID.
 * Autor: Rene Carrillo
 * Fecha: 08.06.2021
 * Fecha de Modificado: 21.07.2021
 * Versión: 1.0.2
 *****************************************************************
 * PARAMETROS DE ENTRADA: MIRId
 * PARAMETROS DE SALIDA: MIRId, Codigo, ProgramaPresupuestario, NombrePlanPeriodo
 *****************************************************************
*/ 
SELECT
	mir.MIRId,
	mir.Ejercicio,
	mir.Codigo, 
	pg.Nombre AS ProgramaPresupuestario,
	pd.NombrePlan + ' ' + (SELECT dbo.GRfnGetFechaConFormato(pd.FechaInicio, 0)) + '- ' + (SELECT dbo.GRfnGetFechaConFormato(pd.FechaFin, 0)) AS NombrePlanPeriodo
FROM
	dbo.MItblMatrizIndicadorResultado mir 
	INNER JOIN MItblPlanDesarrollo pd ON mir.PlanDesarrolloId = pd.PlanDesarrolloId
	INNER JOIN dbo.tblProgramaGobierno pg ON mir.ProgramaPresupuestarioId = pg.ProgramaGobiernoId
WHERE 
	mir.MIRId = @mirId AND mir.EstatusId = 1