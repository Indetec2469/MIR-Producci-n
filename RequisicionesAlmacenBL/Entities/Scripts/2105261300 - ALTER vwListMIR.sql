/****** Object:  View [dbo].[vwListMIR]    Script Date: 5/26/2021 1:29:36 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/* ===========================================
 Author: Rene Carrillo
 Create date: 26/05/2021
 Modified date: 
 Description: View para obtener el Listado de MIR
*/
ALTER VIEW [dbo].[vwListMIR]
AS
SELECT        mir.MIRId, mir.Codigo, mir.Ejercicio, planDesarrollo.NombrePlan, programa.Nombre, dbo.tblControlMaestro.Valor, CASE WHEN CONVERT(date, FechaFinConfiguracion) >= CONVERT(date, GETDATE()) 
                         THEN 'Edición' ELSE 'En proceso' END AS Estatus, CASE WHEN CONVERT(date, FechaFinConfiguracion) >= CONVERT(date, GETDATE()) THEN 1 ELSE 0 END AS Edicion
FROM            dbo.MItblMatrizIndicadorResultado AS mir INNER JOIN
                         dbo.tblPlanNacionalDesarrollo AS planDesarrollo ON mir.PlanNacionalDesarrolloId = planDesarrollo.PlanNacionalDesarrolloId INNER JOIN
                         dbo.tblPlanNacionalDesarrolloEstructura AS estructura ON mir.PlanNacionalDesarrolloEstructuraId = estructura.PlanNacionalDesarrolloEstructuraId INNER JOIN
                         dbo.tblControlMaestro ON estructura.NivelGobiernoId = dbo.tblControlMaestro.ControlId INNER JOIN
                         SACG0000001.dbo.tblProgramaGobierno AS programa ON mir.ProgramaPresupuestarioId = programa.ProgramaGobiernoId
WHERE        (mir.EstatusId <> 3)
GO


