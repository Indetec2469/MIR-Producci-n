SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* ****************************************************************
 * spComboProgramaGobierno
 * ****************************************************************
 * Descripción: Consulta para obtener el combo de Programa Gobierno
 * Autor: Rene Carrillo
 * Fecha: 17.11.2021
 * Versión: 1.0.0
 *****************************************************************
 * PARAMETROS DE ENTRADA: programa.*
 * PARAMETROS DE SALIDA: *
 *****************************************************************
*/

CREATE PROCEDURE [dbo].[spComboProgramaGobierno]
	@ProgramaPresupuestarioId NVARCHAR(6)
AS
SELECT * 
FROM (SELECT programa.*
		FROM tblProgramaGobierno programa
		WHERE programa.ProgramaGobiernoId NOT IN ((SELECT mir.ProgramaPresupuestarioId FROM MItblMatrizIndicadorResultado mir WHERE mir.EstatusId = 1 GROUP BY mir.ProgramaPresupuestarioId, mir.EstatusId))
		UNION ALL
		SELECT programa.*
		FROM tblProgramaGobierno programa
		WHERE programa.ProgramaGobiernoId = @ProgramaPresupuestarioId) AS ProgramaGobierno
ORDER BY ProgramaGobierno.ProgramaGobiernoId ASC