SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER VIEW [dbo].[vwListMatrizConfiguracionPresupuestal]
AS
-- ===========================================
-- Author: Rene Carrillo
-- Create date: 07/06/2021
-- Modified date: 
-- Description:	View para obtener el Listado de Matriz Configuracion Presupuestal
-- ===========================================
SELECT 
	mir.MIRId, 
	mir.Codigo, 
	mir.Ejercicio, 
	pnde.Nombre AS TipoPlanDesarrollo, 
	pg.Nombre AS ProgramaPresupuestario, 
	ISNULL(mcp.PresupuestoPorEjercer, null) AS PresupuestoPorEjercer, 
	ISNULL(mcp.PresupuestoDevengado, null) AS PresupuestoDevengado, 
	CASE WHEN mir.FechaFinConfiguracion > GETDATE() THEN 'Edición' ELSE 'En Proceso' END AS Estatus
FROM MItblMatrizIndicadorResultado mir
	INNER JOIN tblPlanNacionalDesarrolloEstructura pnde ON mir.PlanNacionalDesarrolloEstructuraId = pnde.PlanNacionalDesarrolloEstructuraId
	INNER JOIN SACG0000001.dbo.tblProgramaGobierno pg ON mir.ProgramaPresupuestarioId = pg.ProgramaGobiernoId
	LEFT JOIN MItblMatrizConfiguracionPresupuestal mcp ON mcp.MIRId = mir.MIRId AND mcp.EstatusId = 1
WHERE mir.FechaFinConfiguracion < GETDATE() AND mir.EstatusId = 1