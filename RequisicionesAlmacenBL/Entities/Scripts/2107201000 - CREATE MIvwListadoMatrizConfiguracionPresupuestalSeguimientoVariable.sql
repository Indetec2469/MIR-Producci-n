SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   VIEW [dbo].[MIvwListadoMatrizConfiguracionPresupuestalSeguimientoVariable]
AS
-- ===========================================
-- Author: Rene Carrillo
-- Create date: 20/07/2021
-- Modified date: 
-- Description:	View para obtener el Listado de Matriz Configuracion Presupuestal Seguimiento Variable
-- Versión: 1.0.0
-- ===========================================
SELECT 
	mir.MIRId, 
	mir.Codigo, 
	mir.Ejercicio, 
	pde.Nombre AS TipoPlanDesarrollo,
	pd.NombrePlan + ' ' + (SELECT dbo.GRfnGetFechaConFormato(pd.FechaInicio, 0)) + '- ' + (SELECT dbo.GRfnGetFechaConFormato(pd.FechaFin, 0)) AS NombrePlanPeriodo,
	pg.Nombre AS ProgramaPresupuestario,
	(SELECT COUNT(*) FROM MItblMatrizIndicadorResultadoIndicador miri WHERE miri.MIRId = mir.MIRId) AS Indicadores
FROM MItblMatrizIndicadorResultado mir
	INNER JOIN MItblPlanDesarrollo pd ON mir.PlanDesarrolloId = pd.PlanDesarrolloId
	INNER JOIN MItblPlanDesarrolloEstructura pde ON mir.PlanDesarrolloEstructuraId = pde.PlanDesarrolloEstructuraId
	INNER JOIN tblProgramaGobierno pg ON mir.ProgramaPresupuestarioId = pg.ProgramaGobiernoId
WHERE mir.FechaFinConfiguracion < GETDATE() AND mir.EstatusId = 1
GO