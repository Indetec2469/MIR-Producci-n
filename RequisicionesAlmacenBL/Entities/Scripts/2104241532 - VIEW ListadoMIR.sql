SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER VIEW [dbo].[vwListMIR]
AS
-- ===========================================
-- Author:		Javier Elías
-- Create date: 09/04/2021
-- Modified date: 
-- Description:	View para obtener el Listado de MIR
-- ===========================================
SELECT MIRId AS Id,
       Codigo AS 'Código',
	   Ejercicio,
       NombrePlan AS 'Plan Nacional de Desarrollo',
	   programa.Nombre AS 'Programa Presupuestario',
	   Valor AS 'Tipo de Plan de Desarrollo',
	   CASE WHEN FechaFinConfiguracion > GETDATE() THEN 'Edición' ELSE 'En proceso' END AS Estatus,
	   CASE WHEN FechaFinConfiguracion > GETDATE() THEN 1 ELSE 0 END AS Edicion
FROM RequisicionesAlmacenDatos.dbo.MItblMatrizIndicadorResultado AS mir
     INNER JOIN RequisicionesAlmacenDatos.dbo.tblPlanNacionalDesarrollo AS planDesarrollo ON mir.PlanNacionalDesarolloId = planDesarrollo.PlanNacionalDesarrolloId
	 INNER JOIN RequisicionesAlmacenDatos.dbo.tblPlanNacionalDesarrolloEstructura AS estructura ON mir.PlanNacionalDesarrolloEstructuraId = estructura.PlanNacionalDesarrolloEstructuraId
	 INNER JOIN RequisicionesAlmacenDatos.dbo.tblControlMaestro ON NivelId = ControlId
	 INNER JOIN tblProgramaGobierno AS programa ON mir.ProgramaPresupuestarioId = programa.ProgramaGobiernoId
WHERE mir.EstatusId != 3 --Registros no borrados
GO