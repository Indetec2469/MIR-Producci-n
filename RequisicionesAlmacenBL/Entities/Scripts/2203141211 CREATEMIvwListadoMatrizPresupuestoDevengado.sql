
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER VIEW [dbo].[MIvwListadoMatrizPresupuestoDevengado]
AS
-- ===========================================
-- Author: Alonso Soto
-- Create date: 10/03/2020
-- Modified date: 
-- Description:	View para obtener el Listado de Matriz Presupuesto Devengado
-- ===========================================
SELECT 
	mir.MIRId, 
	mir.Codigo, 
	mir.Ejercicio, 
	pnde.Nombre AS TipoPlanDesarrollo, 
	pg.Nombre AS ProgramaPresupuestario, 
	ISNULL(mcp.PresupuestoPorEjercer, NULL) AS PresupuestoPorEjercer, 
	ISNULL(mcp.PresupuestoDevengado, NULL) AS PresupuestoDevengado, 
	CASE WHEN mir.FechaFinConfiguracion > GETDATE() THEN 'Edición' ELSE 'En Proceso' END AS Estatus
FROM MItblMatrizIndicadorResultado mir
	INNER JOIN MItblPlanDesarrolloEstructura pnde ON mir.PlanDesarrolloEstructuraId = pnde.PlanDesarrolloEstructuraId
	INNER JOIN tblProgramaGobierno pg ON mir.ProgramaPresupuestarioId = pg.ProgramaGobiernoId
	LEFT JOIN MItblMatrizConfiguracionPresupuestal mcp ON mcp.MIRId = mir.MIRId AND mcp.EstatusId = 1 AND mcp.ClasificadorId = 57
WHERE mir.FechaFinConfiguracion < GETDATE() AND mir.EstatusId = 1
GO