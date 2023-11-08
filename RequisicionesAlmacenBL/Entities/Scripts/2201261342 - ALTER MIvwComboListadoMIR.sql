GO

SET QUOTED_IDENTIFIER ON
GO


/* ****************************************************************
 * MIvwComboListadoMIR
 * ****************************************************************
 * Descripción: View para el combo de listado de MIR.
 * Autor: Rene Carrillo
 * Fecha: 30.10.2021
 * Versión: 1.0.0
 *****************************************************************
 * PARAMETROS DE ENTRADA: 
 * PARAMETROS DE SALIDA: MIRId, Codigo + Plan Desarrollo
 *****************************************************************
*/

ALTER VIEW [dbo].[MIvwComboListadoMIR]
AS
SELECT     
	mir.MIRId, mir.Codigo + ' - ' + programaProyecto.Nombre AS Descripcion
FROM            
	MItblMatrizIndicadorResultado mir 
	INNER JOIN tblProgramaGobierno programaProyecto ON mir.ProgramaPresupuestarioId = programaProyecto.ProgramaGobiernoId
WHERE        
	mir.EstatusId = 1 AND mir.FechaFinConfiguracion <= GETDATE()
GO


