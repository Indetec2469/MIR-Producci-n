SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER VIEW [dbo].[vwListPlanNacionalDesarrollo]
AS
-- =================================================================
-- Author:		Javier Elías
-- Create date: 16/03/2021
-- Modified date: 
-- Description:	View para obtener el Listado de Planes Nacionales de Desarrollo
-- =================================================================
SELECT PlanNacionalDesarrolloId AS Codigo,
	   NombrePlan AS Nombre,
	   (SELECT dbo.getFechaConFormato(FechaInicio, 0)) + '- ' + (SELECT dbo.getFechaConFormato(FechaFin, 0)) AS Periodo,
	   CASE WHEN  GETDATE() < FechaInicio THEN 'Próximo' ELSE CASE WHEN GETDATE() > FechaFin THEN 'Finalizado' ELSE 'Vigente' END END AS Estatus
FROM tblPlanNacionalDesarrollo
WHERE EstatusId != 3 --Registros no borrados
GO 